import { useState } from 'react'
import type { FormEvent } from 'react'
import './App.css'
import AgendaForm from './components/AgendaForm';

type ApiResponse = {
    summary?: string
    title?: string
    detail?: string
    errors?: Record<string, string[]>
}

function App() {
    const [notes, setNotes] = useState('')
    const [summary, setSummary] = useState('')
    const [error, setError] = useState('')
    const [isLoading, setIsLoading] = useState(false)

    async function handleSummarize(event: FormEvent<HTMLFormElement>) {
        event.preventDefault()

        if (!notes.trim() || isLoading) {
            return
        }

        setIsLoading(true)
        setError('')
        setSummary('')

        try {
            const response = await fetch('/api/ai/summarize', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    text: notes,
                }),
            })

            // Error responses are not always valid JSON.
            const data: ApiResponse | null = await response
                .json()
                .catch(() => null)

            if (!response.ok) {
                const validationErrors = data?.errors
                    ? Object.values(data.errors).flat().join(' ')
                    : ''

                throw new Error(
                    validationErrors ||
                    data?.detail ||
                    data?.title ||
                    `Request failed (${response.status}).`,
                )
            }

            if (typeof data?.summary !== 'string' || !data.summary.trim()) {
                throw new Error('The backend returned no usable summary.')
            }

            setSummary(data.summary)
        } catch (error: unknown) {
            setError(
                error instanceof Error
                    ? error.message
                    : 'An unexpected error occurred.',
            )
        } finally {
            setIsLoading(false)
        }
    }

    return (
        <main className="meeting-assistant" >
        <h1>AI Meeting Assistant </h1>

            <form onSubmit={handleSummarize}>
                <label htmlFor="meeting-notes">Meeting notes</label>

                <textarea
                    id="meeting-notes"
                    value={notes}
                    onChange={(event) => setNotes(event.target.value)}
                    placeholder="Paste your meeting notes here..."
                    rows={10}
                    maxLength={20000}
                    required
                    disabled={isLoading}
                />

                <p>{notes.length} <span>/ 20,000 characters</span></p>

                <button type="submit" disabled={isLoading || !notes.trim()}>
                    {isLoading ? 'Summarizing...' : 'Summarize'}
                </button>
    </form>

{
    error && (
        <p className="error-message" role = "alert" >
        { error }
            </p>
            )
}

<section aria-live="polite" aria-busy={ isLoading }>
{ isLoading && <p>Generating your summary...</p>}

{
    summary && (
        <>
            <h2>Summary</h2>
            <div className="summary-output">{summary}</div>
        </>
    )
}
</section>
<AgendaForm />
    </main>
    )
}

export default App