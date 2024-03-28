namespace DentistClinic.Services.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity> GetAll(); 
        public TEntity GetById(int id);
        public int Create(TEntity entity);
        public int Update(TEntity entity);
        public int Delete(TEntity entity);
    }
}
