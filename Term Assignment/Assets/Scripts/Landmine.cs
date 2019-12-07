using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete
{
    public class Landmine : MonoBehaviour
    {
        public GameObject flashingObject;
        public Color litColor;
        public Color unLitColor;

        public float minFlash = 0.5f;
        public float maxFlash = 2f;
        public float flashDistance = 2f;        

        List<NPCTankController> enemyList = new List<NPCTankController>();
        Player player;
        
        float currentFlashRate = 2f;
        float closestDistance = 15;
        bool lit = false;
        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            StartCoroutine(AdjustFlash());
        }

        // Update is called once per frame
        void Update()
        {
            CalculateDistance();
            AdjustFlashTime();
        }

        void CalculateDistance()
        {
            float distance = Mathf.Infinity;
            closestDistance = Mathf.Infinity;

            if (enemyList.Count > 0)
            {
                //Check enemy tanks distance in range
                foreach (NPCTankController tank in enemyList)
                {
                    distance = Vector3.Distance(transform.position, tank.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                    }
                }
            }
            else if (Vector3.Distance(transform.position, player.transform.position) < closestDistance)
            {
                //See if player is closer
                closestDistance = Vector3.Distance(transform.position, player.transform.position);
            }
        }

        void AdjustFlashTime()
        {
            float ratio = closestDistance/flashDistance;

            ratio = ratio > 1 ? 1 : ratio;

            currentFlashRate = Mathf.Lerp(minFlash, maxFlash, ratio);
        }

        IEnumerator AdjustFlash()
        {
            while (true)
            {
                yield return new WaitForSeconds(currentFlashRate);

                lit = !lit;
                int lerp = lit ? 1 : 0;
                Color newColor = Color.Lerp(litColor, unLitColor, lerp);

                Renderer rend = flashingObject.GetComponent<Renderer>();
                rend.material.shader = Shader.Find("HDRP/Lit");
                rend.material.SetColor("_BaseColor", newColor);
                rend.material.SetColor("_EmissiveColor", newColor);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            NPCTankController tank = other.gameObject.GetComponent<NPCTankController>();
            if (tank != null)
            {
                enemyList.Add(tank);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            NPCTankController tank = other.gameObject.GetComponent<NPCTankController>();
            if (tank != null)
            {
                enemyList.Remove(tank);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Spawn particle effect

            player.landMineList.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}