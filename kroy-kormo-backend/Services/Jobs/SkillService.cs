using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sob_Need_Server.Context;
using Sob_Need_Server.DTOs.Common;
using Sob_Need_Server.DTOs.Jobs;
using Sob_Need_Server.Interface.Jobs;
using Sob_Need_Server.Models.Jobs;

namespace Sob_Need_Server.Services.Jobs
{
    public class SkillService(AppDbContext context, IMapper mapper) : ISkillService
    {
        private readonly AppDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseDto<IEnumerable<SkillDto>>> GetAllAsync(string? search = null)
        {
            try
            {
                var query = _context.Skills.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(s => s.SkillName.ToLower().Contains(search.ToLower()));
                }

                var skills = await query.OrderBy(s => s.SkillName).ToListAsync();
                var skillDtos = _mapper.Map<IEnumerable<SkillDto>>(skills);

                return new ResponseDto<IEnumerable<SkillDto>>(true, "Skills retrieved successfully", skillDtos);
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<SkillDto>>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<SkillDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var skill = await _context.Skills.FindAsync(id);
                if (skill == null) return new ResponseDto<SkillDto>(false, "Skill not found", null!);

                return new ResponseDto<SkillDto>(true, "Skill retrieved", _mapper.Map<SkillDto>(skill));
            }
            catch (Exception ex)
            {
                return new ResponseDto<SkillDto>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<SkillDto>> CreateAsync(SkillDto entity)
        {
            try
            {
                // Uniqueness check
                if (await _context.Skills.AnyAsync(s => s.SkillName.ToLower() == entity.SkillName.ToLower()))
                    return new ResponseDto<SkillDto>(false, "Skill already exists", null!);

                var skill = _mapper.Map<Skill>(entity);
                skill.CreatedAt = DateTime.UtcNow;
                
                // Ensure slug if missing
                if (string.IsNullOrEmpty(skill.Slug)) 
                    skill.Slug = skill.SkillName.ToLower().Replace(" ", "-");

                await _context.Skills.AddAsync(skill);
                await _context.SaveChangesAsync();

                return new ResponseDto<SkillDto>(true, "Skill created", _mapper.Map<SkillDto>(skill));
            }
            catch (Exception ex)
            {
                return new ResponseDto<SkillDto>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<SkillDto>> UpdateAsync(Guid id, SkillDto entity)
        {
             try
            {
                var skill = await _context.Skills.FindAsync(id);
                if (skill == null) return new ResponseDto<SkillDto>(false, "Skill not found", null!);

                skill.SkillName = entity.SkillName;
                skill.IsActive = entity.IsActive;
                skill.Slug = entity.Slug ?? skill.Slug;
                skill.Description = entity.Description;
                skill.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return new ResponseDto<SkillDto>(true, "Skill updated", _mapper.Map<SkillDto>(skill));
            }
            catch (Exception ex)
            {
                return new ResponseDto<SkillDto>(false, ex.Message, null!);
            }
        }

        public async Task<ResponseDto<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var skill = await _context.Skills.FindAsync(id);
                if (skill == null) return new ResponseDto<bool>(false, "Skill not found", false);

                _context.Skills.Remove(skill);
                await _context.SaveChangesAsync();

                return new ResponseDto<bool>(true, "Skill deleted", true);
            }
            catch (Exception ex)
            {
                return new ResponseDto<bool>(false, ex.Message, false);
            }
        }
    }
}
