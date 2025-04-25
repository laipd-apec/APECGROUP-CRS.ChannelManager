using CRS.ChannelManager.Domain.Dtos;
using CRS.ChannelManager.Library.BaseDto;
using MediatR;

namespace CRS.ChannelManager.FastEndpoint
{
    public class FastChannelRoomType
    {
        public class Create : CRSChannelManagerFastEndpoint<ChannelRoomTypeDto.ChannelRoomTypeCreateDto, ResultBaseDto<long>>
        {
            public Create(IMediator mediator) : base(mediator)
            {

            }

            public override void Configure()
            {
                AllowAnonymous();
                Post("/api/channel-manager/room-type/create");
                Tags("api-crs-channel-room-type");
                Description(x => x.WithTags("api-crs-channel-room-type"));
                Summary(s =>
                {
                    // put your swagger configurations here
                    // per FastEndpoints.Swagger
                });
            }

            public override async Task HandleAsync(ChannelRoomTypeDto.ChannelRoomTypeCreateDto req, CancellationToken ct)
            {
                var result = await Mediator.Send(req, ct);
                await SendOkAsync(new ResultBaseDto<long>(true, string.Empty, result), ct);
            }
        }

        public class CreateList : CRSChannelManagerFastEndpoint<ChannelRoomTypeDto.ChannelRoomTypeCreateListDto, ResultBaseDto<List<long>>>
        {
            public CreateList(IMediator mediator) : base(mediator)
            {

            }

            public override void Configure()
            {
                AllowAnonymous();
                Post("/api/channel-manager/room-type/create-list");
                Tags("api-crs-channel-room-type");
                Description(x => x.WithTags("api-crs-channel-room-type"));
                Summary(s =>
                {
                    // put your swagger configurations here
                    // per FastEndpoints.Swagger
                });
            }

            public override async Task HandleAsync(ChannelRoomTypeDto.ChannelRoomTypeCreateListDto req, CancellationToken ct)
            {
                var result = await Mediator.Send(req, ct);
                await SendOkAsync(new ResultBaseDto<List<long>>(true, string.Empty, result), ct);
            }
        }

        public class Update : CRSChannelManagerFastEndpoint<ChannelRoomTypeDto.ChannelRoomTypeUpdateDto, ResultBaseDto<long>>
        {
            public Update(IMediator mediator) : base(mediator)
            {

            }

            public override void Configure()
            {
                AllowAnonymous();
                Put("/api/channel-manager/room-type/update");
                Tags("api-crs-channel-room-type");
                Description(x => x.WithTags("api-crs-channel-room-type"));
                Summary(s =>
                {
                    // put your swagger configurations here
                    // per FastEndpoints.Swagger
                });
            }

            public override async Task HandleAsync(ChannelRoomTypeDto.ChannelRoomTypeUpdateDto req, CancellationToken ct)
            {
                var result = await Mediator.Send(req, ct);
                await SendOkAsync(new ResultBaseDto<long>(true, string.Empty, result), ct);
            }
        }

        public class UpdateList : CRSChannelManagerFastEndpoint<ChannelRoomTypeDto.ChannelRoomTypeUpdateListDto, ResultBaseDto<List<long>>>
        {
            public UpdateList(IMediator mediator) : base(mediator)
            {

            }

            public override void Configure()
            {
                AllowAnonymous();
                Put("/api/channel-manager/room-type/update-list");
                Tags("api-crs-channel-room-type");
                Description(x => x.WithTags("api-crs-channel-room-type"));
                Summary(s =>
                {
                    // put your swagger configurations here
                    // per FastEndpoints.Swagger
                });
            }

            public override async Task HandleAsync(ChannelRoomTypeDto.ChannelRoomTypeUpdateListDto req, CancellationToken ct)
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
                Delete("/api/channel-manager/room-type/delete/{id:long}");
                Tags("api-crs-channel-room-type");
                Description(x => x.WithTags("api-crs-channel-room-type"));
                Summary(s =>
                {
                    // put your swagger configurations here
                    // per FastEndpoints.Swagger
                });
            }

            public override async Task HandleAsync(CancellationToken ct)
            {
                var id = Route<long>("id");
                var result = await Mediator.Send(new ChannelRoomTypeDto.ChannelRoomTypeDeleteDto() { Id = id }, ct);
                await SendOkAsync(new ResultBaseDto<long>(true, string.Empty, result), ct);
            }
        }

        public class DeleteList : CRSChannelManagerFastEndpoint<ChannelRoomTypeDto.ChannelRoomTypeDeleteListDto, ResultBaseDto<long>>
        {
            public DeleteList(IMediator mediator) : base(mediator)
            {

            }

            public override void Configure()
            {
                AllowAnonymous();
                Delete("/api/channel-manager/room-type/delete-list");
                Tags("api-crs-channel-room-type");
                Description(x => x.WithTags("api-crs-channel-room-type"));
                Summary(s =>
                {
                    // put your swagger configurations here
                    // per FastEndpoints.Swagger
                });
            }

            public override async Task HandleAsync(ChannelRoomTypeDto.ChannelRoomTypeDeleteListDto req, CancellationToken ct)
            {
                var result = await Mediator.Send(req);
                await SendOkAsync(new ResultBaseDto<long>(true, string.Empty, result), ct);
            }
        }

        public class GetOne : CRSChannelManagerFastEndpointWithoutRequest<ResultBaseDto<ChannelRoomTypeDto.ChannelRoomTypeResponseDto>>
        {
            public GetOne(IMediator mediator) : base(mediator)
            {

            }

            public override void Configure()
            {
                {
                    //Verbs(Http.GET);
                    //Routes("/api/booking/hotel/find/{Id:int}");
                    Get("/api/channel-manager/room-type/find/{id:long}");
                    Tags("api-crs-channel-room-type");
                    Description(x => x.WithTags("api-crs-channel-room-type"));
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
                var result = await Mediator.Send(new ChannelRoomTypeDto.ChannelRoomTypeGetOneDto() { Id = id }, ct);
                await SendOkAsync(new ResultBaseDto<ChannelRoomTypeDto.ChannelRoomTypeResponseDto>(true, string.Empty, result), ct);
            }
        }

        public class Search : CRSChannelManagerFastEndpoint<ChannelRoomTypeDto.ChannelRoomTypeSearchDto, ResultBaseDto<List<ChannelRoomTypeDto.ChannelRoomTypeResponseDto>>>
        {
            public Search(IMediator mediator) : base(mediator)
            {

            }

            public override void Configure()
            {
                AllowAnonymous();
                Post("/api/channel-manager/room-type/search");
                Tags("api-crs-channel-room-type");
                Description(x => x.WithTags("api-crs-channel-room-type"));
                Summary(s =>
                {
                    // put your swagger configurations here
                    // per FastEndpoints.Swagger
                });
            }

            public override async Task HandleAsync(ChannelRoomTypeDto.ChannelRoomTypeSearchDto req, CancellationToken ct)
            {
                var result = await Mediator.Send(req, ct);
                var data = new ResultBaseDto<List<ChannelRoomTypeDto.ChannelRoomTypeResponseDto>>();
                if (result == null)
                    data = new ResultBaseDto<List<ChannelRoomTypeDto.ChannelRoomTypeResponseDto>>(true, "Record not found");
                else
                    data = new ResultBaseDto<List<ChannelRoomTypeDto.ChannelRoomTypeResponseDto>>(true, null, result?.Result, result?.Pagination);
                await SendOkAsync(data, ct);
            }
        }
    }
}
