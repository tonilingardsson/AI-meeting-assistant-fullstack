import { useState } from 'react'
import type { FormEvent } from 'react'

type InvitationResponse = {
    invitation?: string
    title?: string
    detail?: string
    errors?: Record<string, string[]>
}

function InvitationForm() {
    const [title, setTitle] = useState('')
    const [purpose, setPurpose] = useState('')
    const [when, setWhen] = useState('')
    const [location, setLocation] = useState('')

    const [invitation, setInvitation] = useState('')
    const [error, setError] = useState('')
    const [isLoading, setIsLoading] = useState(false)

    async function handleSubmit(event: FormEvent<HTMLFormElement>) {
        event.preventDefault()

        if (isLoading) {
            return
        }

        setError('')
        setInvitation('')

        if (
            !title.trim() ||
            !purpose.trim() ||
            !when.trim() ||
            !location.trim()
        ) {
            setError('Please fill in all required fields.')
            return
        }

        setIsLoading(true)

        try {
            const response = await fetch('/api/ai/invitation', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    title: title.trim(),
                    purpose: purpose.trim(),
                    when: when.trim(),
                    location: location.trim()
                }),
            })

        const data: InvitationResponse | null = await response.json().catch(() => null)

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

            if (
                typeof data?.invitation !== 'string' || !data.invitation.trim()
            ) {
                throw new Error('The backend did not return a valid invitation.')
            }

            setInvitation(data.invitation)
        } catch (error: unknown) {
            setError(
                error instanceof Error ? error.message :
                    'An error occurred while submitting the form.',
            )
        } finally {
            setIsLoading(false)
        }
    }

    return (
        <section className="invitation-section">
            <h2>Generate an invitation</h2>
            
        < form onSubmit = { handleSubmit } className = "invitation-form" >
        <label htmlFor="invitation-title" > Meeting title </label>
                <input
                    id="invitation-title"
                    type="text"
                    value={title}
                    onChange={(e) => setTitle(e.target.value)}
                    maxLength={100}
                    required
                    disabled={isLoading}
                />

                <label htmlFor="invitation-purpose">Purpose</label>
                <textarea
                    id="invitation-purpose"
                    value={purpose}
                    onChange={(e) => setPurpose(e.target.value)}
                    maxLength={500}
                    rows={4}
                    required
                    disabled={isLoading}
                />

                <label htmlFor="invitation-when">Meeting time</label>
                <input
                    id="invitation-when"
                    type="text"
                    value={when}
                    onChange={(event) => setWhen(event.target.value)}
                    placeholder="Torsdag 10:00–11:00"
                    maxLength={50}
                    required
                    disabled={isLoading}
                />

                <label htmlFor="invitation-location">Location</label>
                <input
                    id="invitation-location"
                    type="text"
                    value={location}
                    onChange={(event) => setLocation(event.target.value)}
                    placeholder="Konferensrum Orion"
                    maxLength={100}
                    required
                    disabled={isLoading}
                />

                <button type="submit" disabled={isLoading}>
                    {isLoading ? 'Generating invitation...' : 'Generate invitation'}
                </button>
            </form>

            {error && (
                <p className="error-message" role="alert">
                    {error}
                </p>
            )}

            <section aria-live="polite" aria-busy={isLoading}>
                {isLoading && <p>Generating your invitation...</p>}

                {invitation && (
                    <>
                        <h3>Invitation draft</h3>
                        <div className="summary-output">{invitation}</div>
                    </>
                )}
            </section>
        </section>
    )
}

export default InvitationForm