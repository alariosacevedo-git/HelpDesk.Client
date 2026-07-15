namespace HelpDesk.Client.Dto.Documents
{
    public class IdentityResultDto
    {
        public IdentityResultDto()
        {
            ErrorsDescriptionLst = new List<string>();
        }


        public bool IsSucceeded { get; set; }
        public List<string> ErrorsDescriptionLst { get; set; }
    }
}
