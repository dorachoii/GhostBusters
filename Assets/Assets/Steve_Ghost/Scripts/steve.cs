using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class steve : MonoBehaviour
{
    public enum Objects { No_Prop, Axe }
    public Objects props;
    public GameObject[] props_obj; //..............props......................//

    public enum Objects2 { Normal_Head, Head_wihtout_Teeth, Three_Headed }
    public Objects2 heads;
    public GameObject[] heads_obj; //..............props......................//



    public Renderer[] body1;
    public Material[] my_body_materials;
    [Range(0,14)]
    public int Body_Materials; //...............................body material.....................//

    /*public Renderer sword_rend1;
    public Renderer sword_rend2;
    public Material[] sword_material1;
    public Material[] sword_material2;
    [Range(0, 2)]
    public int sword_handle;
    [Range(0, 2)]
    public int sword_body;//..............sword...........//*/

    public Renderer[] R_eye_cover_render;
    public Renderer[] L_eye_cover_render;
    public Material[] eye_cover_mat;
    [Range(0, 8)]
    public int L_Eye_Cover_Color; //...............................body material.....................//
    [Range(0, 8)]
    public int R_Eye_Cover_Color;

    
    
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //Sword//
       /* sword_rend1.material = sword_material1[sword_body];
        sword_rend2.material = sword_material2[sword_handle];*/


        //body material..........//
        for (int i = 0; i < body1.Length; i++)
        {
            body1[i].material = my_body_materials[Body_Materials];
        }

        //Eye Cover material..........//
        for (int i = 0; i < L_eye_cover_render.Length; i++)
        {
            L_eye_cover_render[i].material = eye_cover_mat[L_Eye_Cover_Color];
        }
        for (int i = 0; i < R_eye_cover_render.Length; i++)
        {
            R_eye_cover_render[i].material = eye_cover_mat[R_Eye_Cover_Color];
        }


        //props..................//

        if (props == Objects.No_Prop)
        {
            props_obj[0].SetActive(false);
        }
        else
        {
            props_obj[0].SetActive(true);
        }
        

        //props2..................//

        if (heads == Objects2.Normal_Head)
        {
            heads_obj[0].SetActive(true);
        }
        else
        {
            heads_obj[0].SetActive(false);
        }

        if (heads == Objects2.Head_wihtout_Teeth)
        {
            heads_obj[1].SetActive(true);
        }
        else
        {
            heads_obj[1].SetActive(false);
        }

        if (heads == Objects2.Three_Headed)
        {
            heads_obj[2].SetActive(true);
        }
        else
        {
            heads_obj[2].SetActive(false);
        }
        





    }

}
