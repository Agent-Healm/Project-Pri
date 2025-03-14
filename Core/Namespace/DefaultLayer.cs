// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

namespace DefaultLayer{
    class BitLayer{
        public static int IgnorableLayer(){
            int[] layers = {0, 1, 2, 3, 4, 5};
            return CalculateLayerMask(layers);
        }

        public static int EnemyLayer(){
            int[] layers = {6};
            return CalculateLayerMask(layers);
        }
        
        public static int PlayerLayer(){
            int[] layers = {8};
            return CalculateLayerMask(layers);
        }

        public static int EnvironmentLayer(){
            int[] layers = {10, 11};
            return CalculateLayerMask(layers);
        }

        private static int CalculateLayerMask(int[] arr){
            int layerMask = 0;
            foreach(int layer in arr){
                layerMask |= 1 << layer;
            }
            return layerMask;
        }
    }
}