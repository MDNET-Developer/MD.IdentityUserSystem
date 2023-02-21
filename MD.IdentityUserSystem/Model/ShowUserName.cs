namespace MD.IdentityUserSystem.Model
{
    public static class ShowUserName
    {
        public static string layoutUserName;

        public static void getUserName(string userName)
        {
            userName = layoutUserName;
        }
        
        public static string show()
        {
            return layoutUserName;
        } 
    }
}
