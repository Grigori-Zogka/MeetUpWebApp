﻿using System;
using CloudinaryDotNet.Actions;

namespace RunGroupWebApp.Services
{
	public interface IPhotoService
	{
		Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
		Task<DeletionResult> DeletePhotoAsync(string publicId);
	}
}

