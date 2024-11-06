using CodebridgeTestApp.Dtos;
using CodebridgeTestApp.Models;

namespace CodebridgeTestApp.Mappers
{
    public static class DogMapper
    {
        /// <summary>
        /// Maps DogGetDto to Dog
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static Dog? ToEntity(this DogGetDto dto)
        {
            if (dto is null) return null;

            return new Dog
            {
                name = dto.name,
                color = dto.color,
                tail_length = dto.tail_length,
                weight = dto.weight
            };
        }

        /// <summary>
        /// Maps Dog to DogGetDto
        /// </summary>
        /// <param name="dog"></param>
        /// <returns></returns>
        public static DogGetDto? ToGetDto(this Dog dog)
        {
            if (dog is null) return null;

            return new DogGetDto
            {
                name = dog.name,
                color = dog.color,
                tail_length = dog.tail_length,
                weight = dog.weight
            };
        }

        /// <summary>
        /// Maps DogCreateDto to Dog
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static Dog? ToEntity(this DogCreateDto dto)
        {
            if (dto is null) return null;

            return new Dog
            {
                name = dto.name,
                color = dto.color,
                tail_length = dto.tail_length,
                weight = dto.weight
            };
        }

        /// <summary>
        /// Maps Dog to DogCreateDto
        /// </summary>
        /// <param name="dog"></param>
        /// <returns></returns>
        public static DogCreateDto? ToCreateDto(this Dog dog)
        {
            if (dog is null) return null;

            return new DogCreateDto
            {
                name = dog.name,
                color = dog.color,
                tail_length = dog.tail_length,
                weight = dog.weight
            };
        }
    }

}
