using System;
using System.Threading;
using UnityEngine;

namespace Client
{
    public static class Printer
    {
        private static Semaphore semaphore = new Semaphore(1, 1);

        public static void Log(string message)
        {
            semaphore.WaitOne();
            Debug.Log(message);
            semaphore.Release();
        }
        public static void LogWarning(string message)
        {
            semaphore.WaitOne();
            Debug.LogWarning(message);
            semaphore.Release();
        }
        public static void LogError(string message)
        {
            semaphore.WaitOne();
            Debug.LogError(message);
            semaphore.Release();
        }
    }
}