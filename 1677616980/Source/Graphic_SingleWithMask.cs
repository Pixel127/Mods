using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace BionicIcons
{
    class Graphic_SingleWithMask : Graphic_Single
    {
        public override void Init(GraphicRequest req)
        {
            string path = req.path;
            string mask = null;

            int pos = path.IndexOf("\0");
            if (pos != -1)
            {
                mask = path.Substring(pos + 1);
                path = path.Substring(0, pos);
            }

            this.data = req.graphicData;
            this.path = req.path;
            this.color = req.color;
            this.colorTwo = req.colorTwo;
            this.drawSize = req.drawSize;
            MaterialRequest req2 = default(MaterialRequest);
            req2.mainTex = ContentFinder<Texture2D>.Get(path, true);
            req2.shader = req.shader;
            req2.color = this.color;
            req2.colorTwo = this.colorTwo;
            req2.renderQueue = req.renderQueue;
            req2.shaderParameters = req.shaderParameters;
            if (req.shader.SupportsMaskTex() && mask != null)
            {
                req2.maskTex = ContentFinder<Texture2D>.Get(mask, false);
            }
            this.mat = MaterialPool.MatFrom(req2);
        }

        public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
        {
            return GraphicDatabase.Get<Graphic_SingleWithMask>(path, newShader, this.drawSize, newColor, newColorTwo, this.data, null);
        }

    }
}
