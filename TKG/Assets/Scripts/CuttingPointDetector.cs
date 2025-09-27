using UnityEngine;

public class CuttingPointDetector : MonoBehaviour
{


    [SerializeField] bool use_sample;


    [SerializeField] GameObject sample_line_start_obj;
    [SerializeField] GameObject sample_line_end_obj;

    Vector3 sample_line_start;
    Vector3 sample_line_end;
    public GameObject[] sample_vertices_obj;
    [SerializeField] GameObject sampler_sphere;
    Vector3[] sample_vertices;

     static public Vector3 DetectClosestPointOnLine(Vector3[] vertices, Vector3 line_start, Vector3 line_end)
    {
        ulong index = 0;

        ulong closest_index = 0;
        ulong second_closest_index = 0;
        float closest_distance_sqr = float.MaxValue;
        float second_closest_distance_sqr = float.MaxValue;
        float closest_dot = float.MaxValue;

        Vector3 ab = line_end - line_start;
        Vector3 ab_norm = ab.normalized;
        foreach (Vector3 vert in vertices)
        {
            Vector3 ac = vert - line_start;
            Vector3 bc = vert - line_end;
            float dot1 = Vector3.Dot(ac, ab_norm);
            float dot2 = Vector3.Dot(bc, -ab_norm);
            float distance;
            Vector3 closest_point_on_line;
            if (dot1 < 0)
            {
                distance = ac.sqrMagnitude;
                closest_point_on_line = line_start;

            }
            else if (dot2 < 0)
            {
                distance = bc.sqrMagnitude;
                closest_point_on_line = line_end;

            }
            else
            {
                closest_point_on_line = line_start + ab_norm * dot1;
                distance = (closest_point_on_line - vert).sqrMagnitude;

            }
            if (distance < closest_distance_sqr)
            {
                second_closest_distance_sqr = closest_distance_sqr;
                second_closest_index = closest_index;
                closest_distance_sqr = distance;
                closest_index = index;
                closest_dot = dot1;
            }
            else if (distance < second_closest_distance_sqr)
            {
                second_closest_distance_sqr = distance;
                second_closest_index = index;
            }

            index++;
        }

        //本来はここから最近点1と2を結んだ線分c1c2と、引き数として渡された線分abとでもう一度だけ最近点を計算する必要があるが、
        //今のところ大きな不自然さは感じないのでこれくらいでも良いだろう

      //  Debug.DrawLine(vertices[closest_index], line_start + ab_norm * closest_dot, Color.red);
        return line_start + ab_norm * closest_dot;

    }

}
