namespace Default
{
    class GlobalLayerMask
    {
        public static int IgnorableLayer {
            get {
                int[] layers = { 0, 1, 2, 3, 4, 5 };
                return CalculateLayerMask(layers);
            }
        }

        public static int EnemyLayer {
            get {
                int[] layers = { 6 };
                return CalculateLayerMask(layers);
            }
        }

        public static int PlayerLayer {
            get {
                int[] layers = { 8 };
                return CalculateLayerMask(layers);
            }
        }

        public static int EnvironmentLayer {
            get {
                int[] layers = { 10, 11 };
                return CalculateLayerMask(layers);
            }
        }
        private static int CalculateLayerMask(int[] arr){
            int layerMask = 0;
            foreach (int layer in arr)
            {
                layerMask |= 1 << layer;
            }
            return layerMask;
        }
    }
}