using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using servicedesk.StatusManagementSystem.Domain;
using servicedesk.StatusManagementSystem.Repositories;

namespace servicedesk.StatusManagementSystem.Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;

        public StatusService(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public async Task<IEnumerable<Status>> GetAllAsync(Guid sourceId) 
            => (await _statusRepository.GetAllAsync(sourceId)).ToArray();

        public async Task<Status> GetAsync(Guid id)
            => await _statusRepository.GetAsync(id);

        public async Task CreateAsync(Guid sourceId, string name, string description, int order, int step, bool isFinal, StatusSource source, DateTime createdAt)
        {
            var status = new Status(sourceId, name, description, order, step, isFinal, source, createdAt);
            await _statusRepository.AddAsync(status);
        }
    }
}