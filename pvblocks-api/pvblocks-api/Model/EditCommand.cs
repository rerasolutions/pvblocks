namespace pvblocks_api.Model
{
    public class EditCommand
    {
        public Command Command { get; set; }
        public string CommandDescription { get; set; }
        public PvBlock PvBlock { get; set; }
       
    }
}
