# AI Meeting Assistant

A fullstack application that uses AI to help users plan meetings and turn meeting notes into useful documentation.

Built with React, TypeScript, and ASP.NET Core Web API, the application integrates with the Gemini API through a dedicated backend service.

## Features

### Meeting Notes Summarization

Users can enter meeting notes and generate a structured summary containing:

- Summary
- Decisions
- Action items

The summarization prompt instructs the model to use only information present in the notes and not invent decisions, owners, or deadlines.

### Meeting Agenda Generation

Users provide:

- Meeting title
- Purpose
- Duration in minutes
- Topics
- Participants

The application generates an agenda draft based on these inputs.

### Meeting Invitation Generation

Users provide:

- Meeting title
- Purpose
- Meeting time
- Location

The application generates a professional invitation draft.

Invitations are displayed for review. They are not automatically sent by email or added to a calendar.

The interface uses English labels, while the current backend prompts request AI-generated content in Swedish.

## Technology Stack

| Layer | Technologies |
|---|---|
| Frontend | React, TypeScript, Vite, CSS |
| Frontend tooling | ESLint, TypeScript compiler |
| Backend | C#, ASP.NET Core Web API |
| Validation | Data Annotations and frontend input validation |
| AI integration | Gemini API through a backend AI service |
| HTTP communication | Fetch API in the frontend, HttpClient in the backend |

## Architecture

All AI requests pass through the application's own backend.

```text
React frontend
    ↓
ASP.NET Core Web API
    ↓
Backend services / GeminiService
    ↓
Gemini API
```

The frontend does not call Gemini directly.

During local development, Vite forwards requests beginning with `/api` to the backend.

A shared frontend `postJson` helper handles JSON requests and API error responses. Each component retains responsibility for its form state, input validation, loading indicators, and generated output.

## Project Structure

```text
backend/
  AI-Meeting-Assistant/
    Controllers/
    Models/
    Services/

frontend/
  src/
    components/
      AgendaForm.tsx
      InvitationForm.tsx
    services/
      api.ts
    App.tsx
    App.css
  vite.config.ts
  package.json
```

The summarization form is implemented in `App.tsx`. Agenda and invitation generation use separate components.

## Prerequisites

- A .NET SDK compatible with the backend project's `TargetFramework`
- A Node.js version supported by the installed Vite version
- npm
- A valid Gemini API key and access to the configured model
- Internet access for Gemini requests

Check the backend `.csproj` file for the required .NET target framework.

## Configuration

The backend requires a Gemini API key and the model configuration used by `GeminiService`.

Refer to the backend configuration and `GeminiService` for the exact configuration key names.

Keep the API key on the server side. Do not place it in frontend source code or commit it to the repository.

## Running Locally

### 1. Start the Backend

From the repository root:

```powershell
cd backend/AI-Meeting-Assistant
dotnet restore
dotnet run
```

Check the startup output for:

```text
Now listening on:
```

The frontend development proxy is currently configured to use:

```text
http://localhost:5194
```

If the backend starts at a different address, update the proxy target in `frontend/vite.config.ts`.

Keep the backend terminal running.

### 2. Start the Frontend

Open another terminal from the repository root:

```powershell
cd frontend
npm install
npm run dev
```

Open the address shown beside `Local:` in the Vite output.

The application has been tested locally at:

```text
http://localhost:5173
```

Use the actual address printed in your terminal.

Both servers must remain running to use the AI features.

## Development Proxy

The frontend uses the following Vite configuration:

```ts
import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

export default defineConfig({
  plugins: [react()],
  server: {
    watch: {
      ignored: ['**/.vs/**'],
    },
    proxy: {
      '/api': {
        target: 'http://localhost:5194',
        changeOrigin: true,
      },
    },
  },
})
```

The `.vs` directory is excluded from file watching to avoid conflicts with locked Visual Studio index files.

This proxy applies to the development server. Production deployment requires a separate arrangement for routing frontend API requests to the backend.

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/ai/summarize` | Summarizes meeting notes |
| POST | `/api/ai/agenda` | Generates a meeting agenda |
| POST | `/api/ai/invitation` | Generates a meeting invitation draft |

### Summarize Meeting Notes

Request:

```json
{
  "text": "We discussed the next sprint. Decision: Prioritize the login feature. Action: Anna will write tests before Friday."
}
```

Response structure:

```json
{
  "summary": "Generated summary..."
}
```

### Generate an Agenda

Request:

```json
{
  "title": "Sprint Planning",
  "purpose": "Plan the next sprint and assign work.",
  "durationMinutes": 60,
  "topics": [
    "Review the previous sprint",
    "Prioritize the backlog",
    "Assign tasks"
  ],
  "participants": [
    "Anna",
    "Erik"
  ]
}
```

Response structure:

```json
{
  "agenda": "Generated agenda..."
}
```

Topics and participants are entered one per line in the interface and converted into arrays before submission.

### Generate an Invitation

Request:

```json
{
  "title": "Sprint Planning",
  "purpose": "Plan the next sprint and assign work.",
  "when": "Thursday, 10:00–11:00",
  "location": "Orion Conference Room"
}
```

Response structure:

```json
{
  "invitation": "Generated invitation draft..."
}
```

Response examples show the JSON structure only. Generated text can vary between requests.

## Input Validation

The backend validates requests using Data Annotations. The frontend also applies required fields, length limits, and additional form checks.

| Feature | Validation |
|---|---|
| Summarization | Required text, maximum 20,000 characters |
| Agenda title | Required, maximum 150 characters |
| Agenda purpose | Required, maximum 500 characters |
| Agenda duration | Between 1 and 480 minutes |
| Agenda topics | At least one topic |
| Agenda participants | At least one participant |
| Invitation title | Required, maximum 100 characters |
| Invitation purpose | Required, maximum 500 characters |
| Invitation time | Required, maximum 50 characters |
| Invitation location | Required, maximum 100 characters |

## Error Handling

The interface displays:

- Loading feedback while a request is being processed
- Generated content after a successful request
- An error message when a request fails

Each form disables its inputs and submit button while its request is in progress.

The shared API helper extracts backend validation messages and problem details when available. Components also check that successful responses contain usable generated text.

Gemini may occasionally be temporarily unavailable. Users can wait and submit the request again.

## Build and Code Quality Checks

From the frontend directory:

```powershell
npm run build
npm run lint
```

The build script runs:

```text
tsc -b && vite build
```

The lint script runs:

```text
eslint .
```

## Verification Performed

During development:

- Summarization was tested through the React interface.
- Agenda generation was tested through the React interface.
- Invitation generation was tested through the React interface and PowerShell.
- Frontend build checks completed without reported errors.
- ESLint checks completed without reported errors.

These checks describe manual functional testing and tooling checks, not an automated test suite.

## Limitations

- AI-generated content requires human review.
- Prompt instructions reduce unwanted fabrication but do not guarantee factual accuracy.
- The application generates drafts; it does not make decisions on the user's behalf.
- Invitations are not automatically sent.
- AI features depend on Gemini availability and API access.
- The current setup primarily targets local development.

## Project Context

Developed as part of a fullstack coursework assignment focused on integrating an external AI service with React and ASP.NET Core Web API.

The project demonstrates:

- Frontend-to-backend API integration
- Server-side AI service integration
- Request validation
- Asynchronous form handling
- Loading and error states
- Refactoring repeated HTTP logic into a shared helper
