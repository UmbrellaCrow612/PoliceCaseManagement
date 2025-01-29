import { useState } from "react";
import { Button, StyleSheet, Text, TouchableOpacity, View } from "react-native";
import {
  CameraView,
  CameraType,
  useCameraPermissions,
  BarcodeScanningResult,
} from "expo-camera";
import { useNavigation } from "expo-router";

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

      return null;
    } catch (error) {
      console.error("QR Code parsing error:", error);
      return null;
    }
  }

  function CheckDataFormatAndPush(data: string) {
    const res = processQRCodeData(data);

    if (!res) {
      return;
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
      <View style={styles.permissionContainer}>
        <Text style={styles.permissionText}>Camera doesn't have permission, please grant it</Text>
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
            <Text style={styles.title}>Scan QR Code</Text>
            {qrData && <Text style={styles.qrDataText}>Data: {qrData}</Text>}
          </View>
        </View>
      </CameraView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
  },
  camera: {
    flex: 1,
    width: "100%",
  },
  overlay: {
    position: "absolute",
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
    justifyContent: "center",
    alignItems: "center",
  },
  permissionContainer: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: "rgba(0, 0, 0, 0.6)",
  },
  permissionText: {
    color: "white",
    fontSize: 18,
    marginBottom: 20,
  },
  roundedBox: {
    width: 280,
    height: 280,
    borderRadius: 20,
    borderWidth: 3,
    borderColor: "#fff",
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: "rgba(0, 0, 0, 0.4)",
  },
  title: {
    color: "#fff",
    fontSize: 22,
    fontWeight: "600",
    marginBottom: 10,
  },
  qrDataText: {
    color: "#fff",
    fontSize: 16,
    fontWeight: "400",
    textAlign: "center",
    marginTop: 5,
  },
});
