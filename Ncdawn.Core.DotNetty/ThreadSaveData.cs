using System;
using System.Threading;

namespace Ncdawn.Core.DotNetty
{
    static class ThreadSaveData
    {
        public static void Task1()
        {
            while(true)
            {
                Thread.Sleep(1000);//模拟耗时操作，睡眠1s
                Console.WriteLine("前台线程被调用！");
            }
        }
    }
    
}
