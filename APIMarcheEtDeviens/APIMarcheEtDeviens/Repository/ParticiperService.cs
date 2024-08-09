using Microsoft.EntityFrameworkCore;
using APIMarcheEtDeviens.Data;
using APIMarcheEtDeviens.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;


namespace APIMarcheEtDeviens.Repository
{
    public class ParticiperService : IController<int, ParticiperDto>

    {
        private readonly DataContext _DbContext;
        private IController<int, Participer> _controllerImplementation;
        private readonly IMapper _mapper;

        public ParticiperService(DataContext context, IMapper mapper)
        {
            _DbContext = context;
            _mapper = mapper;
        }

        //Fonction qui récupère et affiche une liste des pensées  
        public async Task<List<ParticiperDto>?> GetAll()
        {
            var medias = await _DbContext.Participer.ToListAsync();
            return medias.Select(media => _mapper.Map<ParticiperDto>(media)).ToList();
        }


        //fonction pour recuperer un seul element depuis l'id
        public async Task<ParticiperDto?> GetById(int id)
        {
            var result = _DbContext.Participer.Find(id);
            if (result == null)
                return null;


            return _mapper.Map<ParticiperDto>(result);

        }

        //fonction pour creer un nouvel element 
        public async Task<List<ParticiperDto?>> Add(ParticiperDto participer)
        {
			var participerInput = _mapper.Map<Participer>(participer);

			_DbContext.Participer.Add(participerInput);
            await _DbContext.SaveChangesAsync();

            return await _DbContext.Participer.Select(media => _mapper.Map<ParticiperDto>(media)).ToListAsync();
        }

        public async Task<List<ParticiperDto>?> Update(int id, ParticiperDto request)
        { 
            var dbParticiper = _DbContext.Participer.Find(id);

            if(dbParticiper == null)
                return null;

            dbParticiper = _mapper.Map<Participer>(request);
            dbParticiper.ParticiperId = id;
            _DbContext.Participer.Update(dbParticiper);

            await _DbContext.SaveChangesAsync();

            return await _DbContext.Participer.Select(media => _mapper.Map<ParticiperDto>(media)).ToListAsync();
        }


        public async Task<List<ParticiperDto>?> DeleteById(int id)
        {
            var result = _DbContext.Participer.Find(id);
            if (result is null)
                return null;

            _DbContext.Participer.Remove(result);
            await _DbContext.SaveChangesAsync();

            return await _DbContext.Participer.Select(media => _mapper.Map<ParticiperDto>(media)).ToListAsync();
        }



    }
}