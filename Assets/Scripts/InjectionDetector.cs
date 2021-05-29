using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Diagnostics;
using UnityEngine;

public class InjectionDetector : MonoBehaviour
{

   //[DllImport("Mitigator", CallingConvention = CallingConvention.Cdecl)]
   //public static extern bool MSSignedOnly();

    /*------------------------------------------------------*/
    private static int loadedCount;
    private static List<string> loadedModuleList = new List<string>();
    private static List<string> initialModuleList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        int initialCount = 0;
        foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
        {
            initialCount += 1;
            initialModuleList.Add(module.ModuleName);
            //UnityEngine.Debug.Log(module.ModuleName + " -> " + module.FileName);
        }
        UnityEngine.Debug.Log("Initial Modules Count: " + initialCount);

        OperatingSystem os_info = Environment.OSVersion;
        String os_ver = os_info.Version.Major.ToString();

        if (os_ver == "10")
        {
            /*
            UnityEngine.Debug.Log("This OS is Windows 10");
            try
            {
                MSSignedOnly();
                bool result = MSSignedOnly();
                UnityEngine.Debug.Log(result);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e.Message);
            }
            */
        }
    }

    // Update is called once per frame
    void Update()
    {
        loadedCount = 0;
        foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
        {
            loadedCount += 1;
            loadedModuleList.Add(module.ModuleName);
            //UnityEngine.Debug.Log(module.ModuleName + " -> " + module.FileName);
        }
        UnityEngine.Debug.Log("Loaded Modules Count: " + loadedCount);

        if (initialModuleList.Count != loadedModuleList.Count)
        {
            UnityEngine.Debug.Log("DLL Injected!");
        }
        loadedModuleList.Clear();
    }
}