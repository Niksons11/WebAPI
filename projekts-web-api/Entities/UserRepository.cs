using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;

namespace WebApi.Entities
{
    public class UserRepository : DbContext
    {
        private readonly UserContext context;
        public UserRepository(UserContext context)
        {
            this.context = context;
        }

        public IQueryable<LimitedResolution> GetResolutions()
        {
            return context.resolutionlist.OrderBy(x => x.Id);
        }
        public LimitedResolution GetResolutionById(int id)
        {
            return context.resolutionlist.Single(x => x.Id == id);
        }



        public int SaveResolution(LimitedResolution entity)
        {

            try
            {
                if (entity.Id == default)
                    context.Entry(entity).State = EntityState.Added;
                else
                    context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
                return entity.Id;
            }
            catch
            {
                return entity.Id - 1;
            }


        }
        public void DeleteResolution(LimitedResolution entity)
        {
            context.resolutionlist.Remove(entity);
            context.SaveChanges();
        }











        public IQueryable<LimitedSize> GetFileSize()
        {
            return context.propertylist.OrderBy(x => x.FileSizeLimit);
        }
        public LimitedSize GetFileSizeById(int id)
        {
            return context.propertylist.Single(x => x.Id == id);
        }


        public int SaveFileSize(LimitedSize entity)
        {

            try
            {
                if (entity.Id == default)
                    context.Entry(entity).State = EntityState.Added;
                else
                    context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
                return entity.Id;
            }
            catch
            {
                return entity.Id - 1;
            }


        }


        public IQueryable<FileProperties> GetExtensions()
        {
            return context.extensionlist.OrderBy(x => x.FileNameExtension);
        }
        public FileProperties GetExtensionById(int id)
        {
            return context.extensionlist.Single(x => x.Id == id);
        }



        public int SaveExtension(FileProperties entity)
        {

            try
            {
                if (entity.Id == default)
                    context.Entry(entity).State = EntityState.Added;
                else
                    context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
                return entity.Id;
            }
            catch
            {
                return entity.Id - 1;
            }


        }
        public void DeleteExtension(FileProperties entity)
        {
            context.extensionlist.Remove(entity);
            context.SaveChanges();
        }




        public IQueryable<User> GetUsers()
        {
            return context.userslist.OrderBy(x => x.Username);
        }

        public User GetUserById(int id)
        {
            return context.userslist.Single(x => x.Id == id);
        }


        public int SaveUser(User entity)
        {

            try
            {
                if (entity.Id == default)
                    context.Entry(entity).State = EntityState.Added;
                else
                    context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
                return entity.Id;
            }
            catch
            {
                return entity.Id - 1;
            }


        }
        public void DeleteUser(User entity)
        {
            context.userslist.Remove(entity);
            context.SaveChanges();
        }

    }
}
