﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

using Common.Core.Numerics;
using Common.Core.Colors;

namespace Common.Unity.Drawing
{

    public enum DRAW_ORIENTATION { XY, XZ };

    public abstract class BaseRenderer
    {
        public static readonly IList<int> CUBE_INDICES = new int[]
        {
            0, 1, 1, 2, 2, 3, 3, 0,
            4, 5, 5, 6, 6, 7, 7, 4,
            0, 4, 1, 5, 2, 6, 3, 7
        };

        public static readonly IList<int> SQUARE_INDICES = new int[]
        {
            0, 1, 1, 2, 2, 3, 3, 0
        };

        protected List<Vector4> Vertices { get; set; }

        protected List<Color> Colors { get; set; }

        protected List<int> Indices { get; set; }

        private Material m_material;

        public BaseRenderer()
        {
            Vertices = new List<Vector4>();
            Colors = new List<Color>();
            Indices = new List<int>();
            LocalToWorld = Matrix4x4.identity;
            Orientation = DRAW_ORIENTATION.XY;
            Color = Color.white;
            CullMode = CullMode.Off;
        }

        public Matrix4x4 LocalToWorld { get; set; }

        public DRAW_ORIENTATION Orientation { get; set;  }

        public Color Color { get; set; }

        public bool ScaleOnZoom { get; set; }

        public CompareFunction ZTest
        {
            get { return (CompareFunction)Material.GetInt("_ZTest"); }
            set { Material.SetInt("_ZTest", (int)value); }
        }

        public CullMode CullMode
        {
            get { return (CullMode)Material.GetInt("_Cull"); }
            set { Material.SetInt("_Cull", (int)value); }
        }

        public bool ZWrite
        {
            get { return Material.GetInt("_ZWrite") == 0 ? false : true; }
            set { Material.SetInt("_ZWrite", value == false ? 0 : 1); }
        }

        public BlendMode SrcBlend
        {
            get { return (BlendMode)Material.GetInt("_SrcBlend"); }
            set { Material.SetInt("_SrcBlend", (int)value); }
        }

        public BlendMode DstBlend
        {
            get { return (BlendMode)Material.GetInt("_DstBlend"); }
            set { Material.SetInt("_DstBlend", (int)value); }
        }

        public int Renderqueue
        {
            get { return Material.renderQueue; }
            set { Material.renderQueue = value; }
        }

        protected Material Material
        {
            get
            {
                if (m_material == null)
                    m_material = new Material(Shader.Find("Hidden/Internal-Colored"));

                return m_material;
            }
        }

        public virtual void Clear()
        {
            Vertices.Clear();
            Colors.Clear();
            Indices.Clear();
        }

        public void SetAllColors(Color color)
        {
            Color = color;
            for (int i = 0; i < Colors.Count; i++)
                Colors[i] = color;
        }

        public void SetLocalToWorld(Matrix4x4f m)
        {
            LocalToWorld = m.ToMatrix4x4();
        }

        public void SetLocalToWorld(float x, float y, float z)
        {
            LocalToWorld = Matrix4x4.TRS(new Vector3(x, y, z), Quaternion.identity, Vector3.one);
        }

        public void SetSegmentIndices(int vertexCount, IList<int> indices)
        {
            int current = Vertices.Count;

            if (indices == null)
            {
                for (int i = 0; i < vertexCount - 1; i++)
                {
                    Indices.Add(i + current);
                    Indices.Add(i + 1 + current);
                }
            }
            else
            {
                for (int i = 0; i < indices.Count; i++)
                        Indices.Add(indices[i] + current);
            }
        }

        public void SetFaceIndices(int vertexCount, IList<int> indices)
        {
            int current = Vertices.Count;

            if (indices == null)
            {
                for (int i = 0; i < vertexCount; i++)
                    Indices.Add(i + current);
            }
            else
            {
                for (int i = 0; i < indices.Count; i++)
                    Indices.Add(indices[i] + current);
            }

        }

        public void Draw()
        {
            Draw(Camera.current);
        }

        public void Draw(Camera camera)
        {
            Draw(camera, LocalToWorld);
        }

        public void Draw(Camera camera, Matrix4x4f localToWorld)
        {
            Draw(camera, localToWorld.ToMatrix4x4());
        }

        public abstract void Draw(Camera camera, Matrix4x4 localToWorld);

    }

}
