using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;

using Nez.Textures;

using System.Collections.Generic;


namespace Nez.Farseer {
	/// <summary>
	/// creates a compound polygon based on an image
	/// </summary>
	public class FSCompoundPolygonBody : FSRenderableBody {
		protected List<Vertices> _verts = new List<Vertices>();


		public FSCompoundPolygonBody(Sprite sprite) : base(sprite) {
		}


		public override void Initialize() {
			base.Initialize();

			uint[] data = new uint[Sprite.SourceRect.Width * Sprite.SourceRect.Height];
			Sprite.Texture2D.GetData(0, Sprite.SourceRect, data, 0, data.Length);

			Vertices verts = PolygonTools.CreatePolygonFromTextureData(data, Sprite.SourceRect.Width);
			verts = SimplifyTools.DouglasPeuckerSimplify(verts, 2);

			List<Vertices> decomposedVerts = Triangulate.ConvexPartition(verts, TriangulationAlgorithm.Bayazit);
			for (int i = 0; i < decomposedVerts.Count; i++) {
				Vertices polygon = decomposedVerts[i];
				polygon.Translate(-Sprite.Center);
			}

			// add the fixtures
			List<FarseerPhysics.Dynamics.Fixture> fixtures = Body.AttachCompoundPolygon(decomposedVerts, 1);

			// fetch all the Vertices and save a copy in case we need to scale them later
			foreach (FarseerPhysics.Dynamics.Fixture fixture in fixtures) {
				_verts.Add(new Vertices((fixture.Shape as PolygonShape).Vertices));
			}
		}


		public override void OnEntityTransformChanged(Transform.Component comp) {
			base.OnEntityTransformChanged(comp);
			if (_ignoreTransformChanges) {
				return;
			}

			// we only care about scale. base handles pos/rot
			if (comp == Transform.Component.Scale) {
				// fetch the Vertices, clear them, add our originals and scale them
				for (int i = 0; i < Body.FixtureList.Count; i++) {
					PolygonShape poly = Body.FixtureList[i].Shape as PolygonShape;
					Vertices verts = poly.Vertices;
					verts.Clear();
					verts.AddRange(_verts[i]);
					verts.Scale(Transform.Scale);
					poly.Vertices = verts;
				}
			}
		}
	}
}