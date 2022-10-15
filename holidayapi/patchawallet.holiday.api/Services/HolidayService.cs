using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace patchawallet.holiday.api
{
    public class HolidayService : IHolidayService
    {
        private readonly IHolidayRepository _repository;
        private readonly IEventBus _eventBus;
        private readonly ILogger<HolidayService> _logger;

        public HolidayService(IHolidayRepository repository,
            IEventBus eventBus,
            ILogger<HolidayService> logger)
        {
            _repository = repository;
            _eventBus = eventBus;
            _logger = logger;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var quantity = await _repository.DeleteAsync(id);
                var eventMessage = new HolidayRemovedIntegrationEvent(id);
                _eventBus.Publish(eventMessage);
                return quantity > 0;
            }
            catch (Exception ex)
            {
                throw new DeleteHolidayException(ex.Message, ex);
            }
        }

        public async Task<List<HolidayVM>> GetAllAsync(int pageIndex = 1, int pageSize = 10, string search = "")
        {
            try
            {
                var holidays = await _repository.GetAllAsync(pageIndex, pageSize, search);
                var vm = holidays.ToVM();
                return vm;
            }
            catch (Exception ex)
            {
                throw new QueryHolidayException(ex.Message, ex);
            }
        }

        public async Task<HolidayVM> GetByIdAsync(int id)
        {
            try
            {
                var holidays = await _repository.GetAsync(id);
                var vm = holidays.ToVM();
                return vm;
            }
            catch (Exception ex)
            {
                throw new QueryHolidayException(ex.Message, ex);
            }
        }

        public async Task<HolidayVM> PostAsync(HolidayInserOrUpdateVM requestVM)
        {
            var holidays = await _repository.GetAllAsync(1, int.MaxValue, string.Empty);
            var entity = requestVM.ToEntity();
            if (holidays.Any(x => x.Date.Date == requestVM.Date))
            {
                throw new HolidayAlreadyCadastredException($"Holiday {requestVM.Description} has already been cadastred with date {requestVM.Date.Date}");
            }
            try
            {
                await _repository.AddAsync(entity);            
            }
            catch (Exception ex)
            {
                throw new PostHolidayException(ex.Message, ex);
            }
            
            var eventMessage = new HolidayCreatedIntegrationEvent(entity.Id, entity.Description, entity.Type, entity.Date, entity.Created, entity.Updated);
            _eventBus.Publish(eventMessage);

            return entity.ToVM();
        }

        public async Task<HolidayVM> UpdateAsync(HolidayInserOrUpdateVM requestVM)
        {
            if (requestVM.Id < 1)
            {
                throw new HolidayNotFoundException($"Invalid value on field id {requestVM.Id}");
            }
            Holiday any = null;
            try
            {
                any = await _repository.GetAsync(1);
            }
            catch (Exception ex)
            {
                throw new QueryHolidayException(ex.Message, ex);
            }
            if (any is null)
            {
                throw new HolidayNotFoundException($"Holiday requested was not found");
            }
            try
            {
                var entity = requestVM.ToEntity();
                var holiday = await _repository.UpdateAsync(entity);
                var vm = holiday.ToVM();

                var eventMessage = new HolidayUpdatedIntegrationEvent(entity.Id, entity.Description, entity.Type, entity.Date, entity.Created, entity.Updated);
                _eventBus.Publish(eventMessage);

                return vm;
            }
            catch (Exception ex)
            {
                throw new PutHolidayException(ex.Message, ex);
            }
        }
    }
}
