namespace CodeHelper
{
    using System;

    public static class GenericExtentions
    {
        /// <summary>
        /// Checks all specified elements for compliance with a condition, and if at least one matches, then returns true 
        /// </summary>
        /// <param name="toDo">Given condition by which compliance will be checked</param>
        /// <param name="check">Parameters to check</param>
        /// <returns>If at least one element matches the condition, returns true</returns>
        public static bool Or<T>(this T self,Func<T,bool> toDo, params T[] check)
        {
            foreach(var item in check)
            {
                if (toDo(item)) return true;
            }
            if(toDo(self)) return true;
            return false;
        }


        /// <summary>
        /// Checks all specified elements for compliance with a condition, and if at least all matches, then returns true 
        /// </summary>
        /// <param name="toDo">Given condition by which compliance will be checked</param>
        /// <param name="check">Parameters to check</param>
        /// <returns>If at least all elements matches the condition, returns true</returns>
        public static bool And<T>(this T self, Func<T,bool> toDo, params T[] check)
        {
            var index = 0;
            foreach (var item in check) if (toDo(item)) index++;
            if (toDo(self)) index++;
            if (index == check.Length +1) return true;
            else return false;
        }
    }
}

