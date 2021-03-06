﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MATAPB.Objects.Tags;
using System.Collections.ObjectModel;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Reflection;
using System.IO;
using SharpDX.D3DCompiler;

namespace MATAPB.Objects
{
    public class TagCollection : AutoDisposeObject
    {
        public TagCollection()
        {

        }

        public Effect CurrentEffect { get; protected set; }
        private EffectConstantBuffer worldConstantBuffer;

        private EffectScalarVariable gzBufferOn;

        private bool _OutputToGZbuffer = true;
        public bool OutputToGZbuffer
        {
            get
            {
                return _OutputToGZbuffer;
            }
            set
            {
                if (gzBufferOn != null)
                {
                    gzBufferOn.Set(value);
                }

                _OutputToGZbuffer = value;
            }
        }

        public InputLayout VertexLayout { get; protected set; }

        private List<Tag> tagsList = new List<Tag>();

        public Object3D AssociatedObject { get; set; }

        public void AddTag(Tag tag)
        {
            tagsList.Add(tag);
            Initialize();
        }

        public void InsertToFirst(Tag tag)
        {
            tagsList.Insert(0, tag);
            Initialize();
        }

        public void AddTag(params Tag[] tags)
        {
            foreach(Tag tag in tags)
            {
                tagsList.Add(tag);
            }
            Initialize();
        }

        public void ClearAndSet(Tag tag)
        {
            tagsList.Clear();
            tagsList.Add(tag);
            Initialize();
        }

        public void RemoveTag(Tag tag)
        {
            tagsList.Remove(tag);
            tag.associatedObject = null;
            Initialize();
        }

        public void ClearTag()
        {
            tagsList.Clear();
            Initialize();
        }

        public int Count { get { return tagsList.Count; } }

        public Tag GetTag(Type type)
        {
            foreach(Tag t in tagsList)
            {
                if (t.GetType() == type)
                    return t;
            }

            return null;
        }

        public void Initialize()
        {
            string text = null;

            foreach(Tag tag in tagsList)
            {
                text += tag.GetShaderText() + "\r\n";
            }

            if (text == null) return;

            if (CurrentEffect != null)
                CurrentEffect.Dispose();

            CurrentEffect = Compile(Build(text));
            InitVertexLayout();
            worldConstantBuffer = CurrentEffect.GetConstantBufferByName("WorldConstantBuffer");
            gzBufferOn = CurrentEffect.GetVariableByName("gzBufferOn").AsScalar();

            gzBufferOn.Set(OutputToGZbuffer);

            foreach (Tag tag in tagsList)
            {
                tag.SetVariables(CurrentEffect);
            }
        }

        public void PrepareToDraw(RenderingContext context)
        {
            if (CurrentEffect == null)
                return;

            foreach(Tag tag in tagsList)
            {
                tag.associatedObject = AssociatedObject;
                tag.Download(context);
            }

            CurrentEffect.GetTechniqueByIndex(0).GetPassByIndex(0).Apply(PresentationBase.GraphicsDevice.ImmediateContext);
            worldConstantBuffer.SetConstantBuffer(context.cbuffer);
            PresentationBase.GraphicsDevice.ImmediateContext.InputAssembler.InputLayout = VertexLayout;
        }

        private void InitVertexLayout()
        {
            if (CurrentEffect == null) return;

            if (VertexLayout != null)
                VertexLayout.Dispose();

            VertexLayout = new InputLayout(
                PresentationBase.GraphicsDevice,
                CurrentEffect.GetTechniqueByIndex(0).GetPassByIndex(0).Description.Signature,
                new[] {
                    new InputElement { SemanticName = "SV_Position", Format = Format.R32G32B32_Float },
                    new InputElement { SemanticName = "NORMAL", Format = Format.R32G32B32_Float, AlignedByteOffset = InputElement.AppendAligned },
                    new InputElement { SemanticName = "TEXCOORD", Format = Format.R32G32_Float, AlignedByteOffset = InputElement.AppendAligned }
                }
                );
        }

        private static string Build(string shaderText)
        {
            string[] lines = shaderText.Split(new char[] { '\r', '\n' });

            bool v = false, vs = false, gs = false, ps = false;
            string variables = null, vertexShader = null, geometryShader = null, pixelShader = null;

            foreach(string line in lines)
            {
                if (line == "")
                    continue;

                switch (line)
                {
                    case "#V":
                        v = true;
                        continue;

                    case "#VS":
                        vs = true;
                        continue;

                    case "#GS":
                        gs = true;
                        continue;

                    case "#PS":
                        ps = true;
                        continue;

                    case "#end":
                        v = false;
                        vs = false;
                        gs = false;
                        ps = false;
                        continue;

                    default:
                        break;
                }

                if (v)
                {
                    variables += line + "\r\n";
                }
                else if(vs)
                {
                    vertexShader += line + "\r\n";
                }
                else if(gs)
                {
                    geometryShader += line + "\r\n";
                }
                else if(ps)
                {
                    pixelShader += line + "\r\n";
                }
            }

            string[] baseText = LoadShaderText("EffectBase.fx").Split('$');
            string result = null;

            for(int i = 0; i < baseText.Length; i++)
            {
                switch (baseText[i])
                {
                    case "V":
                        result += variables;
                        break;

                    case "VS":
                        result += vertexShader;
                        break;

                    case "GS":
                        result += geometryShader;
                        break;

                    case "PS":
                        result += pixelShader;
                        break;

                    default:
                        result += baseText[i];
                        break;
                }
            }

            return result;
        }

        public static Effect Compile(string shader)
        {
            using (ShaderBytecode shaderBytecode = ShaderBytecode.Compile(shader, "fx_5_0", ShaderFlags.None, EffectFlags.None))
            {
                return new Effect(PresentationBase.GraphicsDevice, shaderBytecode);
            }
        }

        public static string LoadShaderText(string name)
        {
            string shader;
            Assembly a = Assembly.GetExecutingAssembly();
            string[] resources = a.GetManifestResourceNames();
            using (StreamReader sr = new StreamReader(a.GetManifestResourceStream("MATAPB.Objects.Tags." + name)))
            {
                shader = sr.ReadToEnd();
            }

            return shader;
        }

        protected override void OnDispose()
        {
            if (CurrentEffect != null) CurrentEffect.Dispose();
            if (VertexLayout != null) VertexLayout.Dispose();
        }
    }
}
