﻿using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TestTemplate.Application.Interfaces;
using TestTemplate.Infrastructure.FileManager.Contexts;
using TestTemplate.Infrastructure.FileManager.Models;

namespace TestTemplate.Infrastructure.FileManager.Services
{
    public class FileManagerService(FileManagerDbContext fileManagerDbContext) : IFileManagerService
    {
        public async Task Create(string name, byte[] content)
        {
            await fileManagerDbContext.Files.AddAsync(new FileEntity(name, content));
        }

        public async Task Delete(string name)
        {
            var file = await fileManagerDbContext.Files.FirstOrDefaultAsync(p => p.Name == name);
            fileManagerDbContext.Files.Remove(file);
        }

        public async Task<byte[]> Download(string name)
        {
            var file = await fileManagerDbContext.Files.FirstOrDefaultAsync(p => p.Name == name);
            return file?.Content;
        }

        public async Task Update(string name, byte[] content)
        {
            var file = await fileManagerDbContext.Files.FirstOrDefaultAsync(p => p.Name == name);
            if (file is null)
            {
                await Create(name, content);
            }
            else
            {
                file.UpdateContent(content);
            }
        }
        public async Task<int> SaveChangesAsync() =>
            await fileManagerDbContext.SaveChangesAsync();
    }
}