"use client";

import { useState } from "react";
import Link from "next/link";
import { Shield, ArrowLeft } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group";

export default function ReportPage() {
  const [isSubmitted, setIsSubmitted] = useState(false);

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    // Here you would typically send the form data to your backend
    // For this example, we'll just set isSubmitted to true
    setIsSubmitted(true);
  };

  return (
    <div className="flex flex-col min-h-screen">
      <header className="bg-blue-900 text-white">
        <div className="container mx-auto px-4 py-6 flex justify-between items-center">
          <Link href="/" className="flex items-center space-x-2">
            <Shield className="h-8 w-8" />
            <span className="text-2xl font-bold">City Police Department</span>
          </Link>
        </div>
      </header>

      <main className="flex-grow container mx-auto px-4 py-8">
        <Link
          href="/"
          className="inline-flex items-center text-blue-600 hover:text-blue-800 mb-6"
        >
          <ArrowLeft className="h-4 w-4 mr-2" />
          Back to Home
        </Link>

        <Card className="w-full max-w-2xl mx-auto">
          <CardHeader>
            <CardTitle className="text-2xl">Report a Non-Emergency</CardTitle>
            <CardDescription>
              Use this form to report non-emergency incidents. For emergencies,
              please call 911 immediately.
            </CardDescription>
          </CardHeader>
          <CardContent>
            {!isSubmitted ? (
              <form onSubmit={handleSubmit} className="space-y-6">
                <div className="space-y-2">
                  <Label htmlFor="name">Full Name</Label>
                  <Input id="name" required />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="email">Email</Label>
                  <Input id="email" type="email" required />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="phone">Phone Number</Label>
                  <Input id="phone" type="tel" required />
                </div>
                <div className="space-y-2">
                  <Label>Incident Type</Label>
                  <RadioGroup
                    defaultValue="property"
                    className="flex flex-col space-y-1"
                  >
                    <div className="flex items-center space-x-2">
                      <RadioGroupItem value="property" id="property" />
                      <Label htmlFor="property">Property Crime</Label>
                    </div>
                    <div className="flex items-center space-x-2">
                      <RadioGroupItem value="traffic" id="traffic" />
                      <Label htmlFor="traffic">Traffic Incident</Label>
                    </div>
                    <div className="flex items-center space-x-2">
                      <RadioGroupItem value="suspicious" id="suspicious" />
                      <Label htmlFor="suspicious">Suspicious Activity</Label>
                    </div>
                    <div className="flex items-center space-x-2">
                      <RadioGroupItem value="other" id="other" />
                      <Label htmlFor="other">Other</Label>
                    </div>
                  </RadioGroup>
                </div>
                <div className="space-y-2">
                  <Label htmlFor="location">Incident Location</Label>
                  <Input id="location" required />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="date">Date of Incident</Label>
                  <Input id="date" type="date" required />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="time">Time of Incident</Label>
                  <Input id="time" type="time" required />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="description">Incident Description</Label>
                  <Textarea id="description" required />
                </div>
                <Button type="submit" className="w-full">
                  Submit Report
                </Button>
              </form>
            ) : (
              <div className="text-center space-y-4">
                <h3 className="text-xl font-semibold text-green-600">
                  Report Submitted Successfully
                </h3>
                <p>
                  Thank you for your report. An officer will review it and
                  contact you if additional information is needed.
                </p>
                <Button onClick={() => setIsSubmitted(false)} className="mt-4">
                  Submit Another Report
                </Button>
              </div>
            )}
          </CardContent>
        </Card>
      </main>

      <footer className="bg-gray-800 text-white py-8">
        <div className="container mx-auto px-4 text-center">
          <p>&copy; 2023 City Police Department. All rights reserved.</p>
          <div className="mt-4 space-y-2 sm:space-y-0">
            <Link
              href="/privacy"
              className="hover:underline block sm:inline-block sm:mr-4"
            >
              Privacy Policy
            </Link>
            <Link
              href="/accessibility"
              className="hover:underline block sm:inline-block"
            >
              Accessibility
            </Link>
          </div>
        </div>
      </footer>
    </div>
  );
}
