
namespace DAL
{
    public interface IDataWorker
    {
        public List <MyAppSeting> ReadSetingFromContext();
        public void WriteSetingFromContext (MyAppSeting newNote);   

    }
}
