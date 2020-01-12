using Commons.Tools.Time;
using System;
using System.Collections.Generic;
using System.Text;

namespace Commons.Tools.KeyGen
{
    public static class KeyGenTool
    {
        /// <summary>
        /// 生成跟uid相关的key  userid|key1|key2
        /// </summary>
        /// <param name="key1"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static string GenKey(string key1, params string[] ps)
        {
            StringBuilder strbuilder = new StringBuilder();
            strbuilder.Append(key1);

            foreach (var key in ps)
            {
                strbuilder.Append("|");
                strbuilder.Append(key);
            }
            return strbuilder.ToString();
        }

        /// <summary>
        /// 生成每天的Key
        /// </summary>
        /// <param name="time"></param>
        /// <param name="key1"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static string GenDayKey(DateTime time, string key1, params string[] ps)
        {
            StringBuilder strbuilder = new StringBuilder();
            strbuilder.Append(key1);

            foreach (var key in ps)
            {
                strbuilder.Append("|");
                strbuilder.Append(key);
            }
            strbuilder.Append("|day");
            strbuilder.Append(time.ToNormalDay());
            return strbuilder.ToString();
        }

        public static string GenWeekKey(DateTime time, string key1, params string[] ps)
        {
            StringBuilder strbuilder = new StringBuilder();
            strbuilder.Append(key1);

            foreach (var key in ps)
            {
                strbuilder.Append("|");
                strbuilder.Append(key);
            }
            strbuilder.Append("|week");
            strbuilder.Append(time.ToNormalWeek());
            return strbuilder.ToString();
        }

        public static string GenUserKey(Int64 userid, params string[] ps)
        {
            StringBuilder strbuilder = new StringBuilder();
            strbuilder.Append(userid.ToString());

            foreach (var key in ps)
            {
                strbuilder.Append("|");
                strbuilder.Append(key);
            }
            return strbuilder.ToString();
        }

        public static string GenUserDayKey(DateTime time, Int64 userid, params string[] ps)
        {
            StringBuilder strbuilder = new StringBuilder();
            strbuilder.Append(userid.ToString());

            foreach (var key in ps)
            {
                strbuilder.Append("|");
                strbuilder.Append(key);
            }
            strbuilder.Append("|day");
            strbuilder.Append(time.ToNormalDay());
            return strbuilder.ToString();
        }

        public static string GenUserWeekKey(DateTime time, Int64 userid, params string[] ps)
        {
            StringBuilder strbuilder = new StringBuilder();
            strbuilder.Append(userid.ToString());

            foreach (var key in ps)
            {
                strbuilder.Append("|");
                strbuilder.Append(key);
            }
            strbuilder.Append("|week");
            strbuilder.Append(time.ToNormalWeek());
            return strbuilder.ToString();
        }
    }
}
