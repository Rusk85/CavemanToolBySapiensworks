namespace CavemanTools.Infrastructure
{
    public enum HandlingType
    {
        /// <summary>
        /// Only message of the type is handled 
        /// </summary>
        Specific,
        /// <summary>
        /// Message and its ancestors are handled
        /// </summary>
        AllDescendants
    }
}