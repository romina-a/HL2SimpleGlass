using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System;

public static class shuffle
{
    public static void Shuffle<T>(this IList<T> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update

    public virtual void end_setting(string s) { Debug.Log("Manager dad called"); }
    protected virtual void load_next_setting() { Debug.Log("manager dad called"); }
}
