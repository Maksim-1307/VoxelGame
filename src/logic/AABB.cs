using System.Collections.Generic;
using System.Collections.Concurrent;
using OpenTK.Mathematics;

namespace VoxelGame.Logic
{
    public struct AABB {
        Vector3 a = new Vector3(0.0f);
        Vector3 b = new Vector3(0.0f);

        Vector3 min (Vector3 v1, Vector3 v2) {
            return Vector3.ComponentMin(v1, v2);
        }
        Vector3 max(Vector3 v1, Vector3 v2)
        {
            return Vector3.ComponentMin(v1, v2);
        }

        public AABB(Vector3 a, Vector3 b)
        {
            this.a = a;
            this.b = b;
        }

        /* Get AABB point with minimal x,y,z */
        Vector3 min()  {
            return min(a, b);
        }

        /* Get AABB point with maximal x,y,z */
        Vector3 max() {
            return max(a, b);
        }

        /* Get AABB dimensions: width, height, depth */
        Vector3 size() {
            return new Vector3(
                fabs(b.X - a.X),
                fabs(b.Y - a.Y),
                fabs(b.Z - a.Z)
            );
        }
        float fabs (float num) {
            if (num > 0) return num; 
            return -num;
        }

        Vector3 center() {
            return (a + b) * 0.5f;
        }

        // /* Multiply AABB size from center */
        // void scale(Vector3 mul)
        // {
        //     Vector3 center = (a + b) * 0.5f;
        //     a = (a - center) * mul + center;
        //     b = (b - center) * mul + center;
        // }

        // /* Multiply AABB size from given origin */
        // void scale(Vector3 mul, Vector3 orig)
        // {
        //     Vector3 beg = min();
        //     Vector3 end = max();
        //     Vector3 center = glm::mix(beg, end, orig);
        //     a = (a - center) * mul + center;
        //     b = (b - center) * mul + center;
        // }

        /* Check if given point is inside */
        public bool contains(Vector3 pos) {
            Vector3 p = min();
            Vector3 s = size();
            return !(pos.X<p.X || pos.Y<p.Y || pos.Z<p.Z || pos.X >= p.X + s.X || pos.Y >= p.Y + s.Y || pos.Z >= p.Z + s.Z);
        }

    }
}




