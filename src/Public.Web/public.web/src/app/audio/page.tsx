'use client'

import { useState } from 'react'
import { Button } from "@/components/ui/button"

export default function AudioCaptcha() {
  const [audioData, setAudioData] = useState<{ bytes: string; id: string } | null>(null)
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const fetchAudio = async () => {
    setIsLoading(true)
    setError(null)
    try {
      const response = await fetch('https://localhost:7052/captcha/audio-questions')
      if (!response.ok) {
        throw new Error('Failed to fetch audio')
      }
      const data = await response.json()
      setAudioData(data)
    } catch (err) {
      setError('Error fetching audio. Please try again.')
      console.error(err)
    } finally {
      setIsLoading(false)
    }
  }

  const playAudio = () => {
    if (audioData) {
      const audioContext = new (window.AudioContext || (window as any).webkitAudioContext)()
      const arrayBuffer = Uint8Array.from(atob(audioData.bytes), c => c.charCodeAt(0)).buffer
      audioContext.decodeAudioData(arrayBuffer, (buffer) => {
        const source = audioContext.createBufferSource()
        source.buffer = buffer
        source.connect(audioContext.destination)
        source.start(0)
      }, (err) => {
        console.error('Error decoding audio data', err)
        setError('Error playing audio. Please try again.')
      })
    }
  }

  return (
    <div className="flex flex-col items-center justify-center min-h-screen p-4">
      <h1 className="text-2xl font-bold mb-4">Audio Captcha</h1>
      <Button onClick={fetchAudio} disabled={isLoading}>
        {isLoading ? 'Loading...' : 'Fetch Audio'}
      </Button>
      {audioData && (
        <Button onClick={playAudio} className="mt-4">
          Play Audio
        </Button>
      )}
      {error && <p className="text-red-500 mt-4">{error}</p>}
      {audioData && <p className="mt-4">Audio ID: {audioData.id}</p>}
    </div>
  )
}

