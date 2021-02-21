using CQRSBlazor.Api.Models;
using MediatR;
using System.Collections.Generic;

namespace CQRSBlazor.Api.Queries
{
    public class GetValuesQuery
    {
        public class Query : IRequest<IEnumerable<string>> { }

        public class Handler : RequestHandler<Query, IEnumerable<string>>
        {
            private readonly FakeDataStore _db;

            public Handler(FakeDataStore db)
            {
                _db = db;
            }

            protected override IEnumerable<string> Handle(Query request)
            {
                return _db.GetAllValues();
            }
        }
    }
}
