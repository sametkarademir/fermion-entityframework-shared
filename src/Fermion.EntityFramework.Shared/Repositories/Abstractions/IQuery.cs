namespace Fermion.EntityFramework.Shared.Repositories.Abstractions;

public interface IQuery<T> where T : class
{
    IQueryable<T> Query();
}