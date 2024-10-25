angular.module('photosApp', ['ngDraggable', 'angularFileUpload'])
	.controller('photosController', function ($scope, $interval, $http, FileUploader)
	{
		$scope.photos = dataObject.photos;
		$scope.urls = urls;
		$scope.isLoaded = true;
		$scope.modal = {};

		$scope.uploader = new FileUploader({
			url: $scope.urls.upload,
			autoUpload: true,
			removeAfterUpload: true
		});

		$scope.uploaderQueue = function()
		{
			return $scope.uploader.queue;
		}

		$scope.deletePhoto = function(photo)
		{
			photo.deleting = true;

			$http({
				method: 'GET',
				url: $scope.getUrl(urls.delete, { 'PHOTO.GUID': photo.guid })
			}).then(function(response)
			{
				if (response.data.isSuccess)
				{
					deleteFromCollection($scope.photos, function (p) { return p.guid == photo.guid});
				}
			});
		}

		$scope.findPhotosOfState = function(stateId)
		{
			return $scope.photos.customWhere(function(p) { return p.stateId == stateId });
		}

		$scope.showJson = function(photo)
		{
			$http({
				method: 'GET',
				url: $scope.getUrl(urls.getJson, { 'PHOTO.GUID': photo.guid })
			}).then(function(response)
			{
				if (response.data.isSuccess)
				{
					$scope.modal.json = response.data.data.json;
					$('#modal-json').modal('show');
				}
			});
		}

		$scope.undoProcessing = function(photo)
		{
			$http({
				method: 'GET',
				url: $scope.getUrl(urls.undoProcessing, { 'PHOTO.GUID': photo.guid })
			}).then(function(response)
			{
				if (response.data.isSuccess)
				{
					updateCollection({
						collection: $scope.photos,
						entrySelector: function (p) { return p.guid == photo.guid },
						updateCallback: function (p) { return p.stateId = 0 }
					});
				}
			});
		}

		$scope.uploader.onSuccessItem = function (fileItem, response, status, headers)
		{
			console.info('onSuccessItem', fileItem, response, status, headers);

			if (!response.isSuccess)
			{
				alert('Не удалось загрузить фото');
				return;
			}

			if (response.data.isNew)
			{
				updateCollection({
					collection: $scope.photos,
					entrySelector: function (p) { return p.guid == response.data.guid },
					updatedEntry: response.data
				})
			}
		};

		$scope.getUrl = function (template, values)
		{
			var result = template;

			var keys = Object.keys(values);

			for (var i = 0; i < keys.length; i++)
			{
				result = result.replace(keys[i], values[keys[i]]);
			}

			return result;
		}

		$scope.getThumbUrl = function (photo)
		{
			if (photo.cloudThubmUrl)
			{
				return photo.cloudThubmUrl;
			}

			return $scope.getUrl(urls.photoThumbnail, { 'PHOTO.GUID': photo.guid });
		}
	});