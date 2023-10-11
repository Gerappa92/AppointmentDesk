# Initial state

`PatientData`
```cs
    public Task<PatientEntity> Get(int id)
    {
        throw new NotImplementedException();
    }
```

`IPatientApi`
```cs
Task<PatientEntity> Get(int id);
```

# Test I - StatusCode OK (200)
`PatientData`
```cs
    public Task<PatientEntity> Get(int id)
    {
        return _api.Get(id);
    }
```

# Test II - StatusCode InternalServerError (500)
Introduce the exception

`ExternalServiceException`

and utilized it in the `PatientData`

```cs
    public async Task<PatientEntity> Get(int id)
    {
        try
        {
            return await _api.Get(id);
        }
        catch (ApiException apiException)
        {
            throw new ExternalServiceException("An error occurred while making an API to Patient Api", apiException);
        }
    }
```

# Test III - StatusCode NotFound (404)
Update the `IPatientApi`  with `ApiResponse`

```cs
    public interface IPatientApi
    {
        [Get("/patient/{id}")]
        Task<ApiResponse<PatientEntity>> Get(int id);
    }
```

utilized it in the `PatientData`

```cs
    public async Task<PatientEntity> Get(int id)
    {
        var response = await _api.Get(id);

        if (response.IsSuccessStatusCode)
        {
            return response.Content;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        throw new ExternalServiceException("An error occurred while making an API to Patient Api", response.Error);
    }
```