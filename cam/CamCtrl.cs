using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCtrl : MonoBehaviour
{
    public AnimationCurve FadeCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.6f, 0.7f, -1.8f, -1.2f), new Keyframe(1, 0));

    private Texture2D tx2d_texture;

    [SerializeField] private float f_alpha = 0;
    private float f_time;
    private float f_duration;
    [SerializeField] private bool  b_done = true;
    [SerializeField] private bool  b_toBlack = true;

    private void Awake()
    {
        b_done = true;
        tx2d_texture = new Texture2D(1, 1);
        tx2d_texture.SetPixel(0, 0, new Color(0, 0, 0, f_alpha));
        tx2d_texture.Apply();
    }

    private void GetSize()
    {
        //half width = (cam.orthographicSize * Screen.width / Screen.height);
    }

    [RuntimeInitializeOnLoadMethod]
    public void DoFade(float length)
    {
        if (!b_done) { print("ERROR: cant do cam fade"); return; }//cant do twice at once

        f_duration  = length;
        f_time      = 0;
        b_done      = false;
    }

    public bool CanFade()
    {
        return b_done;
    }

    public void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tx2d_texture);

        if (b_done) return;

        tx2d_texture.SetPixel(0, 0, new Color(0, 0, 0, f_alpha));
        tx2d_texture.Apply();

        f_time += Time.deltaTime;
        if (!b_toBlack)  f_alpha = FadeCurve.Evaluate(f_time / f_duration);
        else            f_alpha = FadeCurve.Evaluate(1-(f_time / f_duration));


        if (b_toBlack && f_alpha >= 1)
        {
            b_done      = true;
            b_toBlack   = false;

            tx2d_texture.SetPixel(0, 0, new Color(0, 0, 0, 1));
        }
        else if (!b_toBlack && f_alpha <= 0)
        {
            b_done      = true;
            b_toBlack   = true;

            tx2d_texture.SetPixel(0, 0, new Color(0, 0, 0, 0));
        }
    }

    /*
    public void Reset()
    {
        b_done = false;
        f_alpha = 1;
        f_time = 0;
    }

    [RuntimeInitializeOnLoadMethod]
    public void RedoFade(float length)
    {
        f_duration = length;
        Reset();
    }

    public void OnGUI()
    {
        if (b_done) return;
        if (tx2d_texture == null) tx2d_texture = new Texture2D(1, 1);

        tx2d_texture.SetPixel(0, 0, new Color(0, 0, 0, f_alpha));
        tx2d_texture.Apply();

        f_time += Time.deltaTime;
        f_alpha = FadeCurve.Evaluate(f_time / f_duration);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tx2d_texture);

        if (f_alpha <= 0) b_done = true;
    }*/
}
