/**
 *  Thanks for open source!
 *  date：2022-09-03
 *  git : cb4d15273ce2fd914b2317d267d8e366e208cd41
 *  license:https://github.com/ldqk/Masuit.MyBlogs/blob/master/LICENSE 
 *  基于代码版本：b78c483a0dea0d00350bdf44bf788ceb51190e46
 */
using AutoMapper;
using AutoMapper.QueryableExtensions;

using EFCoreSecondLevelCacheInterceptor;

using Masuit.Tools.Models;

using Microsoft.EntityFrameworkCore;

using Morenote.Models.Models.Entity;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace MoreNote.Logic.Infrastructure.Repository;

public static class QueryableExt
{
	/// <summary>
	/// 从二级缓存生成分页集合
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="query"></param>
	/// <param name="page">当前页</param>
	/// <param name="size">页大小</param>
	/// <returns></returns>
	public static PagedList<T> ToCachedPagedList<T>(this IOrderedQueryable<T> query, int page, int size) where T : BaseEntity
	{
		page = Math.Max(1, page);
		var totalCount = query.Count();
		if (page * size > totalCount)
		{
			page = (int)Math.Ceiling(totalCount / (size * 1.0));
		}

		if (page <= 0)
		{
			page = 1;
		}

		var list = query.Skip(size * (page - 1)).Take(size).Cacheable().ToList();
		return new PagedList<T>(list, page, size, totalCount);
	}

	/// <summary>
	/// 从二级缓存生成分页集合
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="query"></param>
	/// <param name="page">当前页</param>
	/// <param name="size">页大小</param>
	/// <returns></returns>
	public static async Task<PagedList<T>> ToCachedPagedListAsync<T>(this IOrderedQueryable<T> query, int page, int size) where T : BaseEntity
	{
		page = Math.Max(1, page);
		var totalCount = query.Count();
		if (page * size > totalCount)
		{
			page = (int)Math.Ceiling(totalCount / (size * 1.0));
		}

		if (page <= 0)
		{
			page = 1;
		}

		var list = await query.Skip(size * (page - 1)).Take(size).Cacheable().ToListAsync();
		return new PagedList<T>(list.ToList(), page, size, totalCount);
	}

	/// <summary>
	/// 生成分页集合
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TDto"></typeparam>
	/// <param name="query"></param>
	/// <param name="page">当前页</param>
	/// <param name="size">页大小</param>
	/// <param name="mapper"></param>
	/// <returns></returns>
	public static PagedList<TDto> ToPagedList<T, TDto>(this IOrderedQueryable<T> query, int page, int size, MapperConfiguration mapper)
	{
		page = Math.Max(1, page);
		var totalCount = query.Count();
		if (page * size > totalCount)
		{
			page = (int)Math.Ceiling(totalCount / (size * 1.0));
		}

		if (page <= 0)
		{
			page = 1;
		}

		var list = query.Skip(size * (page - 1)).Take(size).ProjectTo<TDto>(mapper).NotCacheable().ToList();
		return new PagedList<TDto>(list, page, size, totalCount);
	}

	/// <summary>
	/// 生成分页集合
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TDto"></typeparam>
	/// <param name="query"></param>
	/// <param name="page">当前页</param>
	/// <param name="size">页大小</param>
	/// <param name="mapper"></param>
	/// <returns></returns>
	public static async Task<PagedList<TDto>> ToPagedListAsync<T, TDto>(this IOrderedQueryable<T> query, int page, int size, MapperConfiguration mapper)
	{
		page = Math.Max(1, page);
		var totalCount = await query.CountAsync();
		if (page * size > totalCount)
		{
			page = (int)Math.Ceiling(totalCount / (size * 1.0));
		}

		if (page <= 0)
		{
			page = 1;
		}

		var list = await query.Skip(size * (page - 1)).Take(size).ProjectTo<TDto>(mapper).NotCacheable().ToListAsync();
		return new PagedList<TDto>(list, page, size, totalCount);
	}

	/// <summary>
	/// 从二级缓存生成分页集合
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TDto"></typeparam>
	/// <param name="query"></param>
	/// <param name="page">当前页</param>
	/// <param name="size">页大小</param>
	/// <param name="mapper"></param>
	/// <returns></returns>
	public static PagedList<TDto> ToCachedPagedList<T, TDto>(this IOrderedQueryable<T> query, int page, int size, MapperConfiguration mapper) where TDto : class where T : BaseEntity
	{
		page = Math.Max(1, page);
		var totalCount = query.Count();
		if (page * size > totalCount)
		{
			page = (int)Math.Ceiling(totalCount / (size * 1.0));
		}

		if (page <= 0)
		{
			page = 1;
		}

		var list = query.Skip(size * (page - 1)).Take(size).ProjectTo<TDto>(mapper).Cacheable().ToList();
		return new PagedList<TDto>(list, page, size, totalCount);
	}

	/// <summary>
	/// 从二级缓存生成分页集合
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TDto"></typeparam>
	/// <param name="query"></param>
	/// <param name="page">当前页</param>
	/// <param name="size">页大小</param>
	/// <param name="mapper"></param>
	/// <returns></returns>
	public static async Task<PagedList<TDto>> ToCachedPagedListAsync<T, TDto>(this IOrderedQueryable<T> query, int page, int size, MapperConfiguration mapper) where TDto : class where T : BaseEntity
	{
		page = Math.Max(1, page);
		var totalCount = query.Count();
		if (page * size > totalCount)
		{
			page = (int)Math.Ceiling(totalCount / (size * 1.0));
		}

		if (page <= 0)
		{
			page = 1;
		}

		var list = await query.Skip(size * (page - 1)).Take(size).ProjectTo<TDto>(mapper).Cacheable().ToListAsync();
		return new PagedList<TDto>(list.ToList(), page, size, totalCount);
	}
}