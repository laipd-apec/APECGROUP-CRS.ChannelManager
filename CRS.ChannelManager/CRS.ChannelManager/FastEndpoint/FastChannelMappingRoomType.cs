using CRS.ChannelManager.Domain.Dtos;
using CRS.ChannelManager.Library.BaseDto;
using MediatR;

namespace CRS.ChannelManager.FastEndpoint
{
    public class FastChannelMappingRoomType
    {
        public class Create : CRSChannelManagerFastEndpoint<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeCreateDto, ResultBaseDto<long>>
        {
            public Create(IMediator mediator) : base(mediator)
            {

            }

            public override void Configure()
            {
                AllowAnonymous();
                Post("/api/channel-manager/mapping-room-type/create");
                Tags("api-crs-channel-mapping-room-type");
                Description(x => x.WithTags("api-crs-channel-mapping-room-type"));
                Summary(s =>
                {
                    // put your swagger configurations here
                    // per FastEndpoints.Swagger
                });
            }

            public override async Task HandleAsync(ChannelMappingRoomTypeDto.ChannelMappingRoomTypeCreateDto req, CancellationToken ct)
            {
                var result = await Mediator.Send(req, ct);
                await SendOkAsync(new ResultBaseDto<long>(true, string.Empty, result), ct);
            }
        }

        public class CreateList : CRSChannelManagerFastEndpoint<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeCreateListDto, ResultBaseDto<List<long>>>
        {
            public CreateList(IMediator mediator) : base(mediator)
            {

            }

            public override void Configure()
            {
                AllowAnonymous();
                Post("/api/channel-manager/mapping-room-type/create-list");
                Tags("api-crs-channel-mapping-room-type");
                Description(x => x.WithTags("api-crs-channel-mapping-room-type"));
                Summary(s =>
                {
                    // put your swagger configurations here
                    // per FastEndpoints.Swagger
                });
            }

            public override async Task HandleAsync(ChannelMappingRoomTypeDto.ChannelMappingRoomTypeCreateListDto req, CancellationToken ct)
            {
                var result = await Mediator.Send(req, ct);
                await SendOkAsync(new ResultBaseDto<List<long>>(true, string.Empty, result), ct);
            }
        }

        public class Update : CRSChannelManagerFastEndpoint<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeUpdateDto, ResultBaseDto<long>>
        {
            public Update(IMediator mediator) : base(mediator)
            {

            }

            public override void Configure()
            {
                AllowAnonymous();
                Put("/api/channel-manager/mapping-room-type/update");
                Tags("api-crs-channel-mapping-room-type");
                Description(x => x.WithTags("api-crs-channel-mapping-room-type"));
                Summary(s =>
                {
                    // put your swagger configurations here
                    // per FastEndpoints.Swagger
                });
            }

            public override async Task HandleAsync(ChannelMappingRoomTypeDto.ChannelMappingRoomTypeUpdateDto req, CancellationToken ct)
            {
                var result = await Mediator.Send(req, ct);
                await SendOkAsync(new ResultBaseDto<long>(true, string.Empty, result), ct);
            }
        }

        public class UpdateList : CRSChannelManagerFastEndpoint<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeUpdateListDto, ResultBaseDto<List<long>>>
        {
            public UpdateList(IMediator mediator) : base(mediator)
            {

            }

            public override void Configure()
            {
                AllowAnonymous();
                Put("/api/channel-manager/mapping-room-type/update-list");
                Tags("api-crs-channel-mapping-room-type");
                Description(x => x.WithTags("api-crs-channel-mapping-room-type"));
                Summary(s =>
                {
                    // put your swagger configurations here
                    // per FastEndpoints.Swagger
                });
            }

            public override async Task HandleAsync(ChannelMappingRoomTypeDto.ChannelMappingRoomTypeUpdateListDto req, CancellationToken ct)
            {
                var result = await Mediator.Send(req, ct);
                await SendOkAsync(new ResultBaseDto<List<long>>(true, string.Empty, result), ct);
            }
        }

        public class Delete : CRSChannelManagerFastEndpointWithoutRequest<ResultBaseDto<long>>
        {
            public Delete(IMediator mediator) : base(mediator)
            {

            }

            public override void Configure()
            {
                AllowAnonymous();
                Delete("/api/channel-manager/mapping-room-type/delete/{id:long}");
                Tags("api-crs-channel-mapping-room-type");
                Description(x => x.WithTags("api-crs-channel-mapping-room-type"));
                Summary(s =>
                {
                    // put your swagger configurations here
                    // per FastEndpoints.Swagger
                });
            }

            public override async Task HandleAsync(CancellationToken ct)
            {
                var id = Route<long>("id");
                var result = await Mediator.Send(new ChannelMappingRoomTypeDto.ChannelMappingRoomTypeDeleteDto() { Id = id }, ct);
                await SendOkAsync(new ResultBaseDto<long>(true, string.Empty, result), ct);
            }
        }

        public class DeleteList : CRSChannelManagerFastEndpoint<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeDeleteListDto, ResultBaseDto<long>>
        {
            public DeleteList(IMediator mediator) : base(mediator)
            {

            }

            public override void Configure()
            {
                AllowAnonymous();
                Delete("/api/channel-manager/mapping-room-type/delete-list");
                Tags("api-crs-channel-mapping-room-type");
                Description(x => x.WithTags("api-crs-channel-mapping-room-type"));
                Summary(s =>
                {
                    // put your swagger configurations here
                    // per FastEndpoints.Swagger
                });
            }

            public override async Task HandleAsync(ChannelMappingRoomTypeDto.ChannelMappingRoomTypeDeleteListDto req, CancellationToken ct)
            {
                var result = await Mediator.Send(req);
                await SendOkAsync(new ResultBaseDto<long>(true, string.Empty, result), ct);
            }
        }

        public class GetOne : CRSChannelManagerFastEndpointWithoutRequest<ResultBaseDto<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeResponseDto>>
        {
            public GetOne(IMediator mediator) : base(mediator)
            {

            }

            public override void Configure()
            {
                {
                    //Verbs(Http.GET);
                    //Routes("/api/booking/hotel/find/{Id:int}");
                    Get("/api/channel-manager/mapping-room-type/find/{id:long}");
                    Tags("api-crs-channel-mapping-room-type");
                    Description(x => x.WithTags("api-crs-channel-mapping-room-type"));
                    Summary(s => s.Params["Id"] = "id of the hotel");
                    AllowAnonymous();
                    Summary(s =>
                    {
                        // put your swagger configurations here
                        // per FastEndpoints.Swagger
                    });
                }
            }
            public override async Task HandleAsync(CancellationToken ct)
            {
                var id = Route<long>("id");
                var result = await Mediator.Send(new ChannelMappingRoomTypeDto.ChannelMappingRoomTypeGetOneDto() { Id = id }, ct);
                await SendOkAsync(new ResultBaseDto<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeResponseDto>(true, string.Empty, result), ct);
            }
        }

        public class Search : CRSChannelManagerFastEndpoint<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeSearchDto, ResultBaseDto<List<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeResponseDto>>>
        {
            public Search(IMediator mediator) : base(mediator)
            {

            }

            public override void Configure()
            {
                AllowAnonymous();
                Post("/api/channel-manager/mapping-room-type/search");
                Tags("api-crs-channel-mapping-room-type");
                Description(x => x.WithTags("api-crs-channel-mapping-room-type"));
                Summary(s =>
                {
                    // put your swagger configurations here
                    // per FastEndpoints.Swagger
                });
            }

            public override async Task HandleAsync(ChannelMappingRoomTypeDto.ChannelMappingRoomTypeSearchDto req, CancellationToken ct)
            {
                var result = await Mediator.Send(req, ct);
                var data = new ResultBaseDto<List<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeResponseDto>>();
                if (result == null)
                    data = new ResultBaseDto<List<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeResponseDto>>(true, "Record not found");
                else
                    data = new ResultBaseDto<List<ChannelMappingRoomTypeDto.ChannelMappingRoomTypeResponseDto>>(true, null, result?.Result, result?.Pagination);
                await SendOkAsync(data, ct);
            }
        }
    }
}
