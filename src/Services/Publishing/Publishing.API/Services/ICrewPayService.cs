namespace Publishing.API.Services
{
    public interface ICrewPayService
    {
        void PublishCrewCreatedCommentEvent(string crewId, string crewComent);
        bool SaveCrewComment(string crewId, string crewComent);
    }
}