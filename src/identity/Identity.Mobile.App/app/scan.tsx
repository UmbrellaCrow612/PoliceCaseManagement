import { useState } from "react";
import { Button, StyleSheet, Text, TouchableOpacity, View } from "react-native";
import {
  CameraView,
  CameraType,
  useCameraPermissions,
  BarcodeScanningResult,
} from "expo-camera";
import { Link, useNavigation } from "expo-router";

interface OtpData {
  Id: string;
  CreatedAt: Date;
  ExpiresAt: Date;
  Code: string;
}

function isOtpData(data: any): data is OtpData {
  return (
    typeof data === "object" &&
    data !== null &&
    typeof data.Id === "string" &&
    typeof data.CreatedAt === "string" &&
    typeof data.ExpiresAt === "string" &&
    typeof data.Code === "string"
  );
}

export default function ScanTab() {
  const [back, _] = useState<CameraType>("back");
  const [permission, requestPermission] = useCameraPermissions();
  const { navigate } = useNavigation<any>();
  const [qrData, setQrData] = useState("");

  function processQRCodeData(qrString: string): OtpData | null {
    try {
      const data = JSON.parse(qrString);

      if (isOtpData(data)) {
        return data;
      }

      // Handle other formats if needed

      return null; // Return null if format is not recognized
    } catch (error) {
      console.error("QR Code parsing error:", error);
      return null;
    }
  }

  function CheckDataFormatAndPush(data: string) {
    const res = processQRCodeData(data);

    if (!res) {
      throw new Error("QR Code parsing failed: Unsupported format");
    }

    if (isOtpData(res)) {
      navigate("otp", {
        id: res.Id,
        CreatedAt: res.CreatedAt,
        ExpiresAt: res.ExpiresAt,
        Code: res.Code,
      });
    }
  }

  function onBarcodeScanned(scanningResult: BarcodeScanningResult) {
    CheckDataFormatAndPush(scanningResult.data);
  }

  if (!permission || !permission.granted) {
    return (
      <View>
        <Text>Camera doesn't have permission, please grant it</Text>
        <Button title="Request permission" onPress={requestPermission} />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <CameraView
        style={styles.camera}
        facing={back}
        barcodeScannerSettings={{
          barcodeTypes: ["qr"],
        }}
        onBarcodeScanned={onBarcodeScanned}
      >
        <View style={styles.overlay}>
          <View style={styles.roundedBox}>
            <Text style={styles.title}>Scan QR Code Data: {qrData}</Text>
          </View>
        </View>
      </CameraView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  camera: {
    flex: 1,
  },
  overlay: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: "transparent",
  },
  roundedBox: {
    width: 250,
    height: 250,
    borderRadius: 20,
    borderWidth: 2,
    borderColor: "white",
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: "rgba(255, 255, 255, 0.1)",
  },
  title: {
    color: "white",
    fontSize: 20,
    fontWeight: "bold",
    marginTop: 10,
  },
});
