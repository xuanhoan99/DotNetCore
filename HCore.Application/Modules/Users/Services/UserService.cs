using AutoMapper;
using HCore.Application.Modules.Users.Dtos;
using HCore.Application.Modules.Users.Interfaces;
using HCore.Domain;
using HCore.Domain.Entities;
using HCore.Domain.Repositories;


namespace HCore.Application.Modules.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            var user = new User( dto.FullName, dto.Email, dto.Password,"");
            await _userRepository.AddAsync(user);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            user.UpdateFullName(dto.FullName);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            _ = _userRepository.DeleteAsync(user);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }

}
