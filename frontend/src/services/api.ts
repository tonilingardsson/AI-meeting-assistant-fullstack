/*
    The helper handles sending JSON, parsing the response, 
    and turning unsuccessful HTTP responses into errors. 
    response.ok indicates a status between 200 and 299
 */

type ApiErrorResponse = {
    title?: string
    detail?: string
    errors?: Record<string, string[]>
}

export async function postJson<TResponse>(
    endpoint: string,
    body: unknown,
): Promise<TResponse> {
    const response = await fetch(endpoint, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(body),
    })

    // Error responses are not always valid JSON.
    const data: unknown = await response.json().catch(() => null)

    // If the response is not ok, throw an error with the message from the API or a generic message. 
    if (!response.ok) {
        const apiError = data as ApiErrorResponse | null

        // If the API returned validation errors, it will be formattted into a single string.
        const validationErrors = apiError?.errors
            ? Object.values(apiError.errors).flat().join(' ')
            : ''

        // Throw an error with the validation errors, API error detail, or a generic message.
        throw new Error(
            validationErrors ||
            apiError?.detail ||
            apiError?.title ||
            `Request failed (${response.status}).`,
        )
    }

    if (data === null) {
        throw new Error('The backend returned no usable data or invalid JSON response.')
    }

    return data as TResponse
}