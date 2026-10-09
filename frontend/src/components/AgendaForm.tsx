import { useState } from 'react'
import type { FormEvent } from 'react'

type AgendaResponse = {
    agenda?: string
    title?: string
    detail?: string
    errors?: Record<string, string[]>
}

function toList(value: string): string[] {
    return value
        .split('\n')
        .map((item) => item.trim())
        .filter((item) => item.length > 0)
}

function AgendaForm() {
    const [title, setTitle] = useState('')
    const [purpose, setPurpose] = useState('')
    const [durationMinutes, setDurationMinutes] = useState(60)
    const [topics, setTopics] = useState('')
    const [participants, setParticipants] = useState('')

    const [agenda, setAgenda] = useState('')
    const [error, setError] = useState('')
    const [isLoading, setIsLoading] = useState(false)

    async function handleSubmit(event: FormEvent<HTMLFormElement>) {
        event.preventDefault()

        if (isLoading) {
            return
        }

        setError('')
        setAgenda('')

        const topicList = toList(topics)
        const participantList = toList(participants)

        if (
            !title.trim() ||
            !purpose.trim() ||
            topicList.length === 0 ||
            participantList.length === 0
        ) {
            setError(
                'Enter a title, purpose, at least one topic, and at least one participant.',
            )
            return
        }

        if (
            !Number.isInteger(durationMinutes) ||
            durationMinutes < 1 ||
            durationMinutes > 480
        ) {
            setError('Duration must be a whole number between 1 and 480 minutes.')
            return
        }

        setIsLoading(true)

        try {
            const response = await fetch('/api/ai/agenda', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    title: title.trim(),
                    purpose: purpose.trim(),
                    durationMinutes,
                    topics: topicList,
                    participants: participantList,
                }),
            })

            const data: AgendaResponse | null = await response
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

            if (typeof data?.agenda !== 'string' || !data.agenda.trim()) {
                throw new Error('The backend returned no usable agenda.')
            }

            setAgenda(data.agenda)
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
        <section className="agenda-section">
            <h2>Generate an agenda</h2>

            <form onSubmit={handleSubmit}>
                <label htmlFor="agenda-title">Meeting title</label>
                <input
                    id="agenda-title"
                    type="text"
                    value={title}
                    onChange={(event) => setTitle(event.target.value)}
                    maxLength={150}
                    required
                    disabled={isLoading}
                />

                <label htmlFor="agenda-purpose">Purpose</label>
                <textarea
                    id="agenda-purpose"
                    value={purpose}
                    onChange={(event) => setPurpose(event.target.value)}
                    maxLength={500}
                    rows={3}
                    required
                    disabled={isLoading}
                />

                <label htmlFor="agenda-duration">Duration in minutes</label>
                <input
                    id="agenda-duration"
                    type="number"
                    value={durationMinutes}
                    onChange={(event) =>
                        setDurationMinutes(event.target.valueAsNumber)
                    }
                    min={1}
                    max={480}
                    step={1}
                    required
                    disabled={isLoading}
                />

                <label htmlFor="agenda-topics">Topics — one per line</label>
                <textarea
                    id="agenda-topics"
                    value={topics}
                    onChange={(event) => setTopics(event.target.value)}
                    rows={4}
                    required
                    disabled={isLoading}
                />

                <label htmlFor="agenda-participants">Participants — one per line</label>
                <textarea
                    id="agenda-participants"
                    value={participants}
                    onChange={(event) => setParticipants(event.target.value)}
                    rows={4}
                    required
                    disabled={isLoading}
                />

                <button type="submit" disabled={isLoading}>
                    {isLoading ? 'Generating agenda...' : 'Generate agenda'}
                </button>
            </form>

            {error && (
                <p className="error-message" role="alert">
                    {error}
                </p>
            )}

            <section aria-live="polite" aria-busy={isLoading}>
                {isLoading && <p>Generating your agenda...</p>}

                {agenda && (
                    <>
                        <h3>Agenda draft</h3>
                        <div className="summary-output">{agenda}</div>
                    </>
                )}
            </section>
        </section>
    )
}

export default AgendaForm