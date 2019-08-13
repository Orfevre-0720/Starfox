using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Movement5.
/// </summary>
public class movement5 : MonoBehaviour {

    public bool move = false;
    public float speed = 40.0f;//自機の速度
    public float rotate_speed = 100.0f;//最後にかける数字
    [Range(0.0f, 100.0f)]
    public float forward_speed = 30;

    public float move_timer = 1.0f;
float rot_speed = 100f;//自機の回転速度

    public GameObject N_B;//Particleを宣言



    // Start is called before the first frame update
    void Start() {
    
    }

    // Update is called once per frame
    void Update() {
        float dx = Input.GetAxis("Horizontal");
        float dy = Input.GetAxis("Vertical");

        Vector3 rot = this.transform.localEulerAngles;
        Vector3 pos = this.transform.position;

        if (move == true) {
            pos.z += forward_speed * Time.deltaTime;
            this.transform.position = pos;
        }
        //-----------------------------------------------------------------------------------------------------------------回転と移動
        //-------------------------------------------------------------------------------------------------------------------------


        //何も操作されていない時0に戻す
        if ((dx == 0.0f) && (dy == 0.0f)) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rot.x, rot.y, 0), 150.0f * Time.deltaTime);//原点を向く
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), 150.0f * Time.deltaTime);//Zを処理してからXYを0にすることにした
            move_timer = 1.0f;//RESET
        } else {
            //  move_timer += 1 * Time.deltaTime;//時間経過でスピードに傾斜をつける
            //---------------------------------------------------------------------------------------回転
            //最初にZの回転

            //↓最短距離を通るためXが動いてしまうらしい
            //this.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rot.x,rot.y, Mathf.Clamp(-dx * rot_speed, -40.0F, 40.0F)), rotate_speed * Time.deltaTime);

            //↓指定した角度まで一気に向いてしまう
            //this.transform.rotation = Quaternion.Euler(rot.x, rot.y, Mathf.Clamp(-dx * rot_speed * rotate_speed * Time.deltaTime, -40.0F, 40.0F));

            //一気に向いてしまう　変数で調整が必要
            //    transform.eulerAngles = new Vector3(rot.x, rot.y, Mathf.Clamp(-dx * 20.0f, -40.0F, 40.0F));

            //Lerpを使ってみたけど変だった
            //    this.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Mathf.Clamp(-dy * rot_speed, -70.0F, 70.0F), Mathf.Clamp(dx *rot_speed, -70.0F, 70.0F), rot.z), 5f * Time.deltaTime);

            /*Zの回転のあとにXYの機体の回転(本当はこっちのほうが滑らか)
            this.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(
            Mathf.Clamp(-dy *rot_speed, -70.0F, 70.0F),
            Mathf.Clamp(dx *rot_speed, -70.0F, 70.0F),
            rot.z
            ), rotate_speed * Time.deltaTime);
            */
            /*回転に制限を付ける方法が見つからず　ずっと回転してしまう
            float r_speed_stage_x = dx * 200 * Time.deltaTime;
            float r_speed_stage_y = -dy * 200 * Time.deltaTime;

            Quaternion rotX = Quaternion.AngleAxis(Mathf.Clamp(r_speed_stage_y,-70,70), Vector3.right);
            Quaternion rotY = Quaternion.AngleAxis(Mathf.Clamp(r_speed_stage_x,-70,70), Vector3.up);
            // 現在の回転値と合成
            Quaternion newRotation = rotY * rotX * transform.rotation;

            // 新しい回転値を設定
            this.transform.rotation = newRotation;

            */
            /*いっきに回転して色々おかしい
            Vector3 XandY = new Vector3(Mathf.Clamp(dx * 5, -70.0F, 70.0F), Mathf.Clamp(dy * 5, -70.0F, 70.0F), rot.z);

            Quaternion rotation = Quaternion.LookRotation(XandY);
            transform.rotation = rotation;
            */
          
            //XYの回転 問題は旋回が急になってしまうこと(なぜかはわかる)
             this.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Mathf.Clamp(-dy * rot_speed, -70.0F, 70.0F), Mathf.Clamp(dx *rot_speed, -70.0F, 70.0F), rot.z), 2.5f * Time.deltaTime);
            //Zの回転
            this.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rot.x, rot.y, Mathf.Clamp(-dx * rot_speed, -30.0F, 30.0F)), 3f * Time.deltaTime);

            //--------------------------------------------------------------------------------------移動

            Vector3 target_position = new Vector3(
              Mathf.Clamp(transform.position.x + (dx * Mathf.Abs(dx)  * speed  * Time.deltaTime), -45, 45),
                Mathf.Clamp(transform.position.y + (dy * Mathf.Abs(dy) * speed * Time.deltaTime), -20, 20), pos.z);

               this.transform.position = Vector3.Lerp(this.transform.position,target_position, 0.5f);
            //this.transform.position = Vector3.MoveTowards(transform.position, target_position, 0.5f);

        }
        //-------------------------------------------------------------------------------------------------------------回転と移動
       
         //ビームの発射
        if (Input.GetKeyDown(KeyCode.Space)){
            Instantiate(N_B, new Vector3(pos.x, pos.y, pos.z), transform.rotation);
        }


        //デバッグ用
        if (Input.GetKey(KeyCode.R)) {
            this.transform.position = new Vector3(0, 0, 0);
        }

        //ローリング(仮)
        if ((Input.GetKeyDown(KeyCode.R)) | (Input.GetKeyDown(KeyCode.L))) {
            transform.Rotate(new Vector3(0, 0, 1), -dx * 40.0f);
        }

        float keisan = 2.5f * Time.deltaTime;
        Debug.Log(keisan);
    }



}

