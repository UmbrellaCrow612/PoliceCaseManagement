'use client';

import { useState } from "react";
import Link from "next/link";
import { useForm } from "react-hook-form";
import { Shield, ArrowLeft } from 'lucide-react';
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

type FormData = {
  name: string;
  email: string;
  phone: string;
  incidentType: string;
  location: string;
  date: string;
  time: string;
  description: string;
};

export default function ReportPage() {
  const [isSubmitted, setIsSubmitted] = useState(false);
  const { register, handleSubmit, formState: { errors }, reset } = useForm<FormData>();

  const onSubmit = (data: FormData) => {
    // Here you would typically send the form data to your backend
    console.log(data);
    setIsSubmitted(true);
    reset();
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
              <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                  <div className="space-y-2">
                    <Label htmlFor="name">Full Name</Label>
                    <Input 
                      id="name" 
                      {...register("name", { required: "Full name is required" })}
                    />
                    {errors.name && <p className="text-red-500 text-sm">{errors.name.message}</p>}
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="email">Email</Label>
                    <Input 
                      id="email" 
                      type="email" 
                      {...register("email", { 
                        required: "Email is required",
                        pattern: {
                          value: /\S+@\S+\.\S+/,
                          message: "Invalid email address"
                        }
                      })}
                    />
                    {errors.email && <p className="text-red-500 text-sm">{errors.email.message}</p>}
                  </div>
                </div>
                <div className="space-y-2">
                  <Label htmlFor="phone">Phone Number</Label>
                  <Input 
                    id="phone" 
                    type="tel" 
                    {...register("phone", { 
                      required: "Phone number is required",
                      pattern: {
                        value: /^[0-9]{10}$/,
                        message: "Invalid phone number, please use 10 digits"
                      }
                    })}
                  />
                  {errors.phone && <p className="text-red-500 text-sm">{errors.phone.message}</p>}
                </div>
                <div className="space-y-2">
                  <Label>Incident Type</Label>
                  <RadioGroup
                    {...register("incidentType", { required: "Please select an incident type" })}
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
                  {errors.incidentType && <p className="text-red-500 text-sm">{errors.incidentType.message}</p>}
                </div>
                <div className="space-y-2">
                  <Label htmlFor="location">Incident Location</Label>
                  <Input 
                    id="location" 
                    {...register("location", { required: "Incident location is required" })}
                  />
                  {errors.location && <p className="text-red-500 text-sm">{errors.location.message}</p>}
                </div>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                  <div className="space-y-2">
                    <Label htmlFor="date">Date of Incident</Label>
                    <Input 
                      id="date" 
                      type="date" 
                      {...register("date", { required: "Date is required" })}
                    />
                    {errors.date && <p className="text-red-500 text-sm">{errors.date.message}</p>}
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="time">Time of Incident</Label>
                    <Input 
                      id="time" 
                      type="time" 
                      {...register("time", { required: "Time is required" })}
                    />
                    {errors.time && <p className="text-red-500 text-sm">{errors.time.message}</p>}
                  </div>
                </div>
                <div className="space-y-2">
                  <Label htmlFor="description">Incident Description</Label>
                  <Textarea 
                    id="description" 
                    {...register("description", { 
                      required: "Description is required",
                      minLength: {
                        value: 20,
                        message: "Description must be at least 20 characters long"
                      }
                    })}
                  />
                  {errors.description && <p className="text-red-500 text-sm">{errors.description.message}</p>}
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
          <div className="mt-4 space-x-4">
            <Link
              href="/privacy"
              className="hover:underline inline-block"
            >
              Privacy Policy
            </Link>
            <Link
              href="/accessibility"
              className="hover:underline inline-block"
            >
              Accessibility
            </Link>
          </div>
        </div>
      </footer>
    </div>
  );
}

