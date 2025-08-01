// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("KMVcLYwzA+GMRbskVTm598a5VNL/96fl76vbAkAnaNtwc85pvJfXUDevFngyI02BQJk/eL89Y3JIjmqKtCH5YIdyePmfPjxZU6fKct2Vj6P8l1dm4EbXqRgtNysB2sEcLp7G2WXm6OfXZebt5WXm5ucu2LqQkWBsne4fOaTHmkYtF3pY279ZNUrrN/jz8XvZ5iwyGFvTh3mWk/sfA5P+7aVF/L0z2ZbtO2R0jYisyuLX3v9PrOamfsqeuxFplmTDMYFcAoqXsyWx7DW7StGNq06BdtJFNSYT/C2i2maz5jQlHfUPXXcA2zxs9bM8hkI612Xmxdfq4e7NYa9hEOrm5ubi5+Q/HVaXTA6n90Z/AzY01oIGhf3Q+l3y1FtnDJHC0OXk5ufm");
        private static int[] order = new int[] { 3,7,5,6,10,7,7,8,13,11,10,12,13,13,14 };
        private static int key = 231;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
