using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Operations.Contracts;
using Operations.DTOs;
using Operations.Models;

namespace Operations.Repository
{
    public class OutlineDescriptionRepository : GenericPilotPlantRepository<OutlineDescription>, IOutlineDescriptionRepository
    {
        public OutlineDescriptionRepository(PilotPlantEntities dbContext)
            : base(dbContext)
        {
        }
        public async Task<int> AddFromDtoAsync(OutlineDescriptionDto dto)
        {
            var outline = TransferToDescription(dto);
            return await AddAsync(outline);
        }

        public async Task<OutlineDescriptionDto> GetDtoAsync(int? id)
        {
            OutlineDescription outlineDescription = await FindAsync(i => i.Id == id);
            if (outlineDescription == null)
                return null;

            var dto = TransferToDto(outlineDescription);
            IOutlineTopicRepository tr = new OutlineTopicRepository(DbContext);
            dto.OutlineTopics = await tr.GetAllAsync();
            return dto;
        }

        public  OutlineDescription FindByName(string name)
        {
            return DbContext.OutlineDescriptions.FirstOrDefault(o => o.Name == name);
        }

        private OutlineDescriptionDto TransferToDto(OutlineDescription outlineDescription)
        {
            OutlineDescriptionDto dto = new OutlineDescriptionDto
            {
                OutlineTopicId = outlineDescription.OutlineTopicId,
                Name = outlineDescription.Name,
                OutlineTopicName = outlineDescription.OutlineTopicName,
                ValuesState = outlineDescription.ValuesState,
                Checked = outlineDescription.Checked,
                UpdateBy = outlineDescription.UpdateBy,
                LastUpdate = outlineDescription.LastUpdate
            };
            return dto;
        }

        private OutlineDescription TransferToDescription(OutlineDescriptionDto dto)
        {
            OutlineDescription outline = new OutlineDescription
            {
                OutlineTopicId = dto.OutlineTopicId,
                Name = dto.Name,
                ValuesState = 0,
                Checked = false,
                UpdateBy = dto.UpdateBy,
                LastUpdate = dto.LastUpdate
            };
            outline.OutlineTopicName = DbContext.OutlineTopics.First(e => e.Id == outline.OutlineTopicId).Name;
            return outline;
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    DbContext.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}